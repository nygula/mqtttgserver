using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewLife.Configuration;
namespace Castle.Mqtt.TGServer
{
    public class ServerConfig:Config<ServerConfig>
    {
        [DisplayName("系统服务名称")]
        public string ServerName { set; get; } = "CastleMqttTGServer";

        [DisplayName("MQTT服务器地址")]
        public string MqttServer { set; get; } = "mqtt://192.168.1.70:1883";


        [DisplayName("MQTT帐号")]

        public string MqttUserName { set; get; } = "admin";
        [DisplayName("MQTT密码")]

        public string MqttPassword { set; get; } = "123456";


        [DisplayName("MQTT订阅主题")]

        public string PullTopTic { set; get; } = "/xindoo/tg/v2/#";
            
    }
}
