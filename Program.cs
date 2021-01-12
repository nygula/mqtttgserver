using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewLife.Agent;
using NewLife.MQTT;
using NewLife;
using NewLife.Log;
using NewLife.Serialization;
using NewLife.Threading;
using NewLife.MQTT.Messaging;
using NewLife.Security;

namespace Castle.Mqtt.TGServer
{
    class Program
    {
        static void Main(string[] args) =>new MyService().Main(args);

    }

  

    public class MyService : ServiceBase
    {
        public MqttClient _MqttClient;
        public MyService()
        {
         
            XTrace.UseConsole();
            ServiceName = ServerConfig.Current.ServerName;



        }

        TimerX timer1;
      

        // TimerX timer1;
        public void InitMqttClient()
        {
            var cli = new MqttClient()
            {
                ClientId = Guid.NewGuid() + "",
                Server = ServerConfig.Current.MqttServer, //  mqtt://192.168.1.222:1883
                KeepAlive = 6,                             //  保证6秒内是在线的.
                UserName = ServerConfig.Current.MqttUserName, // admin
                Password = ServerConfig.Current.MqttPassword,  // 123456
                Log = XTrace.Log,
                Timeout = 6000
            };
            XTrace.WriteLine($"[配置ClientToJson]:{cli.ToJson()}");
            cli.ConnectAsync();
            cli.SubscribeAsync(ServerConfig.Current.PullTopTic, (e) =>
            {
                XTrace.WriteLine("[订阅]:" + ServerConfig.Current.PullTopTic + ".[消息]:" + e.Payload.ToStr());
            });
            _MqttClient = cli;
        }

     
        public void CheckMqttClient()
        {
            _MqttClient.Server.Host = ServerConfig.Current.MqttServer;
            XTrace.WriteLine("[检查]:"+_MqttClient.ToJson());
        }
       
        protected override void StartWork(string reason)
        {
            
            
             InitMqttClient();
            
          
            timer1 = new TimerX(a => CheckMqttClient(), null, 2000, 2000) { Async = true };
            base.StartWork(reason);
        }
        protected override void StopWork(string reason)
        {
            if (!_MqttClient.Disposed)
            {
                _MqttClient.DisconnectAsync();
                _MqttClient.Dispose();
            }
           timer1.Dispose();

            base.StopWork(reason);
        }
    }
}
