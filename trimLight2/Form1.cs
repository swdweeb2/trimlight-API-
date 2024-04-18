using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Http;
using System.Runtime.Remoting.Contexts;
using System.IO;
using System.Runtime.CompilerServices;
using System.Security.Policy;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Net.Http.Headers;
using System.Net;
using System.Xml;
using Newtonsoft.Json;
namespace trimLight2
{

    public partial class Form1 : Form
    {
        config configObj = null;
        public Form1()
        {
            InitializeComponent();
            configObj = new config();
        }

        private async void startButton_Click(object sender, EventArgs e)
        {
            getDevices();
        }

        private Dictionary<string, string> buildHeaders(HttpWebRequest request)
        {
            // 1.Concatenate strings: "Trimlight|<S-ClientId>|<S-Timestamp>"
            // 2.Compute the HMAC-SHA256 of the string concatenated in step 1.The secret key for HMAC - SHA256 is clientSecret.
            // 3.The value of the accessToken is the base64 encoding of the computed HMAC - SHA256 value.
            // for example, "Trimlight|tester|1713166849256"

            TimeSpan elapsedTime = DateTime.Now - new DateTime(1970, 1, 1);
            string accessString = "Trimlight|" + configObj.ClientId + "|" + Convert.ToInt64(elapsedTime.TotalMilliseconds).ToString();
            Encoding enc = Encoding.UTF8;
            HMACSHA256 encryptEngine = new HMACSHA256(enc.GetBytes(configObj.ClientSecret));
            byte[] result = encryptEngine.ComputeHash(enc.GetBytes(accessString));
            string accessToken = Convert.ToBase64String(result);

            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("authorization", accessToken);
            headers.Add("S-ClientId", configObj.ClientId);
            headers.Add("S-Timestamp", Convert.ToInt64(elapsedTime.TotalMilliseconds).ToString());
            return headers;
        }

        private void getDevices()
        {
            string devicesURI = "/v1/oauth/resources/devices";
            HttpWebRequest request =
                (HttpWebRequest)WebRequest.Create(new Uri(configObj.BaseURI + devicesURI));
            request.Method = "GET";

            Dictionary<string, string> headers = buildHeaders(request);
            List<string> keyList = new List<string>(headers.Keys);
            foreach (string key in keyList)
            {
                //textBox1.AppendText("adding header -> " + key + ":" + headers[key] + Environment.NewLine);
                request.Headers.Add(key, headers[key]);
            }
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream responseStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(responseStream);
            string devices = reader.ReadToEnd();
            trimlightDevices thisDevice = JsonConvert.DeserializeObject<trimlightDevices>(devices);
            string formattedJSON = JsonConvert.SerializeObject(devices, Newtonsoft.Json.Formatting.Indented);
            textBox1.AppendText("Device info:" + Environment.NewLine);
            textBox1.AppendText("\tname = " + thisDevice.payload.data[0].name + Environment.NewLine);
            textBox1.AppendText("\tdeviceId = " + thisDevice.payload.data[0].deviceId + Environment.NewLine);
            textBox1.AppendText("\tConnectivity = " + thisDevice.payload.data[0].connectivity.ToString() + Environment.NewLine);
            textBox1.AppendText("\tState = " + thisDevice.payload.data[0].state.ToString() + Environment.NewLine);
            textBox1.AppendText("\tFirmware version = " + thisDevice.payload.data[0].fwVersionName + Environment.NewLine);
        }
    }
}
