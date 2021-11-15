using System;
using System.Collections.Generic;

namespace ObserverPattern
{
    public enum OrderState
    {
        CRETAED,CONFIRMED,CANCELLED,CLOSED
    }
    
    public   class Order
    {
       public event Action<string> OrderStateChanged;//event
        //order closed 
        public event Action<string> OrderClosed;
        string orderId;
        OrderState currentState;
        public Order()
        {
            orderId = Guid.NewGuid().ToString();
            currentState = OrderState.CRETAED;
        }

        public Order(string closed)
        {
            orderId = Guid.NewGuid().ToString();
            currentState = OrderState.CLOSED;
        }
        public void ChangeState(OrderState newState)
        {
            this.currentState = newState;
            NotifyAll();
        }
        void NotifyAll()
        {
            if (OrderStateChanged != null)
            {
                this.OrderStateChanged.Invoke(this.orderId);//one->Many (Multicast Delegate Instance)
            }
        }

        ////Subscribe,Register
        //public void Add_OrderStateChanged(Action observerAddress)
        //{
        //    this.OrderStateChanged += observerAddress;//System.Delegate.Combine
        //}
        ////UnSubScribe
        //public void Remove_OrderStateChanged(Action observerAddress)
        //{
        //    this.OrderStateChanged -= observerAddress;//System.Delegate.Remove
        //}

    }

    public class EmailNotifificationSystem
    {
        public void SendMail(string evtData) { Console.WriteLine($"Email Sent  {evtData}"); }
    }
    public class SMSNotificationSystem
    {
        public void SendSMS(string evtData) {
            Console.WriteLine($"SMS Sent  {evtData}");
        }
    }

    public class NotifySystem
    {
        public void Notified(string evtData)
        {
            Console.WriteLine($"Order closed  { evtData}");
        }
    }

    public class WhatsAppNotification
    {
        public void SendWhatsApp(string evtData) { Console.WriteLine($"WhatsApp Sent {evtData}"); }
    }

    class Program
    {
        static void Main(string[] args)
        {
            EmailNotifificationSystem _emailSystem = new EmailNotifificationSystem();
            SMSNotificationSystem _smsSystem = new SMSNotificationSystem();
            WhatsAppNotification _whatsAppSystem = new WhatsAppNotification();
            NotifySystem _notifySystem = new NotifySystem();

            Action<string> _emailObserver = new Action<string>(_emailSystem.SendMail);

            Action<string> _smsObserver = new Action<string>(_smsSystem.SendSMS);

            Action<string> _whatsAppObserver = new Action<string>(_whatsAppSystem.SendWhatsApp);

            Action<string> _notifyObserver = new Action<string>(_notifySystem.Notified);

            Order _order1 = new Order();
            _order1.OrderStateChanged += _emailObserver;// Add_OrderStateChanged(_emailObserver)
            _order1.OrderStateChanged += _smsObserver;
            _order1.OrderStateChanged += _whatsAppObserver;

            Order _order2 = new Order("closed");
            _order2.OrderStateChanged += _notifyObserver;
            System.Threading.Tasks.Task.Delay(5000).Wait();
            _order2.ChangeState(OrderState.CLOSED);

            _order1.ChangeState(OrderState.CONFIRMED);
            System.Threading.Tasks.Task.Delay(1000).Wait();
            _order1.ChangeState(OrderState.CANCELLED);
            System.Threading.Tasks.Task.Delay(3000).Wait();
            _order1.ChangeState(OrderState.CONFIRMED);
            System.Threading.Tasks.Task.Delay(5000).Wait();
            _order1.ChangeState(OrderState.CLOSED);
        }
    }
}
