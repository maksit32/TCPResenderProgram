using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;


//ретранслирует файлы, отправленные 1 клиеном другим оставшимся
//192.168.0.13
IPAddress localAddr = IPAddress.Parse("192.168.0.12");
//слушающий сокет
TcpListener listen = new TcpListener(localAddr, 4572);

//массив клиентов
List<TcpClient> Clients = new List<TcpClient>();
//индекс ОТПРАВИТЕЛЯ
int indexSender = 0;




try
{
    //запускаем сервер
    listen.Start();
    Console.WriteLine("Сервер запущен!");
    // тут UI выпрыгивает (отдельно крутится прослушивание)
    WaitIncomingConnectionsAsync(listen);
    await ServeClients(); //тут останавливаем программу
    Console.WriteLine("Нажмите любую клавишу для завершения");
    Console.ReadKey();

}
finally
{
    //закрываю слушающий сокет
    listen.Stop();
}






async Task WaitIncomingConnectionsAsync(TcpListener listen)
{
    while (true)
    {
        //получить клиента
        TcpClient client = await listen.AcceptTcpClientAsync();

        //добавляем клиента в список клиентов
        lock (Clients)
        {
            Clients.Add(client);
        }
        Console.WriteLine($"Подключение с {client.Client.RemoteEndPoint} установлено.");
    }
}
//функции считывания блоков данных
async Task<byte[]> BufferedReadBlock(Stream stream)
{

    byte[] prefixBuf = await BufferedRead(stream, sizeof(int));
    int length = BitConverter.ToInt32(prefixBuf);
    byte[] message = await BufferedRead(stream, length);
    return message;
}
async Task<byte[]> BufferedRead(Stream stream, int length)
{
    byte[] buffer = new byte[length];
    for (int progress = 0; progress < length;)
    {
        int chunk = await stream.ReadAsync(buffer, progress, length - progress);
        progress += chunk;
    }
    return buffer;
}
//обслужить клиентов
async Task ServeClients()
{
    while (true)
    {
        //буффер
        byte[] buffer = new byte[0];
        //количество клиентов доступных для отправки
        if (Clients.Count > 0)
        {
            for (int i = 0; i < Clients.Count; i++)
            {
                //если клиент доступен для receive
                if (Clients[i].Available > 0)
                {
                    //читаем его данные
                    buffer = await BufferedReadBlock(Clients[i].GetStream());
                    //переводим в строку
                    string str = Encoding.UTF8.GetString(buffer);
                    //забрали индекс отправителя
                    indexSender = i;
                    break;
                }
            }
        }
        //если что-то отправлено
        if (buffer.Length > 0)
        {
                for (int i = 0; i < Clients.Count; i++)
                {
                    //отправить всем, кроме отправляющего
                    if (i != indexSender)
                    {
                        //получить поток и отправить информацию
                        await Clients[i].GetStream().WriteAsync(BitConverter.GetBytes(buffer.Length));
                        await Clients[i].GetStream().WriteAsync(buffer, 0, buffer.Length);
                    }
                }
        }
    }

}
