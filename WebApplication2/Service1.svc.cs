using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Web;

namespace WebApplication2
{
    // ПРИМЕЧАНИЕ. Команду "Переименовать" в меню "Рефакторинг" можно использовать для одновременного изменения имени класса "Service1" в коде, SVC-файле и файле конфигурации.
    // ПРИМЕЧАНИЕ. Чтобы запустить клиент проверки WCF для тестирования службы, выберите элементы Service1.svc или Service1.svc.cs в обозревателе решений и начните отладку.
    
    public class Service1 : IService1
    {
       
        private String makeDate(String s)
        {
            String res = "{ d '";
            res += s;
            res += "'} ";
            return res;
        }

        private bool isPaid(String name, String token)
        {
            string url = "http://payment-service-uni.apphb.com/PaymentREST.svc/checkPa.." + name + "&token=" + token +
            "&ip=217.21.43.95&useragent=" + "";
            using (var webClient = new WebClient())
            {
                // Выполняем запрос по адресу и получаем ответ в виде строки 
                var response = webClient.DownloadString(url);
                var JSONObj = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize < System.Collections.Generic.Dictionary < string, string>>(response);
                string isCorrect = JSONObj["d"];
                if (isCorrect.Equals("True"))
                    return true;
                else
                    return false;
            }
           
        }

        public String configureQuerryString(string author, string book, string date, string name, string password, string token)
        {
         
            if (!isPaid(name, token))
                return null;
           
            string querryString = "select * from books";
            if (author != "")
            {
                querryString += " where author='";
                querryString += author;
                querryString += "'";

            }

            if (book != "")
            {
                if (author != "")
                    querryString += " and book_name='";
                else
                    querryString += " where book_name='";
                querryString += book;
                querryString += "'";
            }

            if (date != "")
            {
                if (author != "" || book != "")
                {
                    querryString += " and date = ";
                    querryString += makeDate(date);
                }
                else
                {
                    querryString += " where date = ";
                    querryString += makeDate(date);
                }
            }

            return querryString;
        }
    }
}
