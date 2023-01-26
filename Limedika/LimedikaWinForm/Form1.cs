using Common.Entities;
using Common.Interfaces;

namespace LimedikaWinForm
{
    public partial class Form1 : Form
    {
        private readonly IClientService _clientService;
        private readonly IClientPageService _clientPageService;
        private readonly ILogService _logService;
        private readonly IBufferedFileUploadService _bufferedFileUploadService;
        private List<Client> Clients;
        private List<Log> Logs;
        private int MAX_PAGE;
        private int CURRENT_PAGE;
        private const int PER_PAGE = 30;
        private TYPE SHOW;
        private enum TYPE
        {
            CLIENTS,
            LOGS
        }

        public Form1(IClientService clientService, 
            IClientPageService clientPageService,
            ILogService logService,
            IBufferedFileUploadService bufferedFileUploadService)
        {
            _clientService = clientService;
            _clientPageService = clientPageService;
            _logService = logService;
            _bufferedFileUploadService = bufferedFileUploadService;

            InitializeComponent();

            label1.Text = "0/0";
        }

        /// <summary>
        /// Klientų sąrašas (atnaujinti)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void button1_ClickAsync(object sender, EventArgs e)
        {
            textBox1.Clear();
            Clients = await _clientService.GetClients();

            MAX_PAGE = Clients.Count / PER_PAGE;
            CURRENT_PAGE = 1;
            label1.Text = $"{CURRENT_PAGE}/{MAX_PAGE}";
            SHOW = TYPE.CLIENTS;

            textBox1.AppendText(string.Format("{0,-40}|{1,-50}|{2,-30}\r\n","Pavadinimas", "Adresas", "Pašto kodas"));
            foreach (var client in Clients.Skip(PER_PAGE * (CURRENT_PAGE-1)).Take(PER_PAGE))
            {
                textBox1.AppendText(string.Format("{0,-40}|{1,-50}|{2,-30}\r\n", client.Name, client.Address, client.PostCode));
            }
        }

        /// <summary>
        /// Importuoti klientus
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void button2_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                var stream = openFileDialog1.OpenFile();
                var clients = await _bufferedFileUploadService.UploadFile(stream);
                if(clients != null)
                {
                    await _clientPageService.ImportClients(clients);
                    button1_ClickAsync(sender, e);
                }
            }
        }

        /// <summary>
        /// Atnaujinti pašto indeksus
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void button3_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            await _clientPageService.UpdatePostCodes();
            button1_ClickAsync(sender, e);
        }

        /// <summary>
        /// Veiksmų istorija
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void button4_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            Logs = await _logService.GetLogs();

            MAX_PAGE = Logs.Count / PER_PAGE;
            CURRENT_PAGE = 1;
            label1.Text = $"{CURRENT_PAGE}/{MAX_PAGE}";
            SHOW = TYPE.LOGS;

            Logs = Logs.OrderBy(x => x.TimeStamp).ToList();
            textBox1.AppendText(string.Format("{0,-40}|{1,-40}\r\n",
                              "Laiko žyma", "Vartotojo veiksmas"));
            foreach (var log in Logs.Skip(PER_PAGE * (CURRENT_PAGE - 1)).Take(PER_PAGE))
            {
                textBox1.AppendText(string.Format("{0,-40}|{1,-40}\r\n", log.TimeStamp, log.UserAction));
            }
        }

        /// <summary>
        /// Atgal
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button5_Click(object sender, EventArgs e)
        {
            textBox1.Clear();

            CURRENT_PAGE = CURRENT_PAGE > 1 ? CURRENT_PAGE - 1 : CURRENT_PAGE;
            label1.Text = $"{CURRENT_PAGE}/{MAX_PAGE}";

            if (SHOW == TYPE.CLIENTS)
            {
                textBox1.AppendText(string.Format("{0,-40}|{1,-50}|{2,-30}\r\n", "Pavadinimas", "Adresas", "Pašto kodas"));
                foreach (var client in Clients.Skip(PER_PAGE * (CURRENT_PAGE - 1)).Take(PER_PAGE))
                {
                    textBox1.AppendText(string.Format("{0,-40}|{1,-50}|{2,-30}\r\n", client.Name, client.Address, client.PostCode));
                }
            }
            else
            {
                textBox1.AppendText(string.Format("{0,-40}|{1,-40}\r\n",
                  "Laiko žyma", "Vartotojo veiksmas"));
                foreach (var log in Logs.Skip(PER_PAGE * (CURRENT_PAGE - 1)).Take(PER_PAGE))
                {
                    textBox1.AppendText(string.Format("{0,-40}|{1,-40}\r\n", log.TimeStamp, log.UserAction));
                }
            }
        }

        /// <summary>
        /// Pirmyn
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button6_Click(object sender, EventArgs e)
        {
            textBox1.Clear();

            CURRENT_PAGE = CURRENT_PAGE < MAX_PAGE ? CURRENT_PAGE + 1 : CURRENT_PAGE;
            label1.Text = $"{CURRENT_PAGE}/{MAX_PAGE}";

            if (SHOW == TYPE.CLIENTS)
            {
                textBox1.AppendText(string.Format("{0,-40}|{1,-50}|{2,-30}\r\n", "Pavadinimas", "Adresas", "Pašto kodas"));
                foreach (var client in Clients.Skip(PER_PAGE * (CURRENT_PAGE - 1)).Take(PER_PAGE))
                {
                    textBox1.AppendText(string.Format("{0,-40}|{1,-50}|{2,-30}\r\n", client.Name, client.Address, client.PostCode));
                }
            }
            else
            {
                textBox1.AppendText(string.Format("{0,-40}|{1,-40}\r\n",
                  "Laiko žyma", "Vartotojo veiksmas"));
                foreach (var log in Logs.Skip(PER_PAGE * (CURRENT_PAGE - 1)).Take(PER_PAGE))
                {
                    textBox1.AppendText(string.Format("{0,-40}|{1,-40}\r\n", log.TimeStamp, log.UserAction));
                }
            }
        }
    }
}