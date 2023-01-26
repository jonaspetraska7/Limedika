using Common.Interfaces;

namespace LimedikaWinForm
{
    public partial class Form1 : Form
    {
        private readonly IClientService _clientService;
        private readonly IClientPageService _clientPageService;
        private readonly ILogService _logService;
        private readonly IPostCodeService _postCodeService;
        private readonly IBufferedFileUploadService _bufferedFileUploadService;

        public Form1(IClientService clientService, 
            IClientPageService clientPageService,
            ILogService logService,
            IPostCodeService postCodeService,
            IBufferedFileUploadService bufferedFileUploadService)
        {
            _clientService = clientService;
            _clientPageService = clientPageService;
            _logService = logService;
            _postCodeService = postCodeService;
            _bufferedFileUploadService = bufferedFileUploadService;

            InitializeComponent();
        }

        /// <summary>
        /// Klientų sąrašas (atnaujinti)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void button1_ClickAsync(object sender, EventArgs e)
        {
            textBox1.Clear();
            var clients = await _clientService.GetClients();
            textBox1.AppendText(string.Format("{0,-40}|{1,-50}|{2,-30}\r\n","Pavadinimas", "Adresas", "Pašto kodas"));
            foreach (var client in clients)
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
            var logs = await _logService.GetLogs();
            textBox1.AppendText(string.Format("{0,-40}|{1,-40}\r\n",
                              "Laiko žyma", "Vartotojo veiksmas"));
            foreach (var log in logs)
            {
                textBox1.AppendText(string.Format("{0,-40}|{1,-40}\r\n", log.TimeStamp, log.UserAction));
            }
        }
    }
}