using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FirstSimpleChatApp
{



    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private HubConnection hubConnection;
        private Message message;
       
        public MainWindow()
        {
            InitializeComponent();

            Action<Message> action1 = (message) =>
            {
                listview.Items.Add($"{message.Text}: {message.Date}");
            };

            hubConnection = new HubConnectionBuilder()
                .WithUrl("https://signal-r-app.herokuapp.com/chat").Build();
            message = new Message();

            hubConnection.On<Message>("Send", action1.Invoke);

            Action action = async () =>
            {
                await hubConnection.StartAsync();
            };

            action.Invoke();
        }

        public void Button_Click(object sender, RoutedEventArgs e)
        {
            message.Text = textbox.Text;
            message.Date = DateTime.Now.ToString();
            listview.Items.Add(message.Text);

            Action action = async () =>
            {
                await hubConnection.SendAsync("SendMessage", message);
            };

            action.Invoke();
        }
             
    }   
}
