using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyPaking
{
    internal class ParkingSensor
    {
        [JsonProperty("sensorId")]
        public string sensorId { get; set; }
        [JsonProperty("isOccupied")]
        public bool IsOccuppied { get; set; }
        [JsonProperty("timestamp")]
        public DateTime timestamp { get; set; }
        public void setIsOcuppied(bool isOccuppied) {
            this.IsOccuppied = isOccuppied;
        }
        public ParkingSensor(string sensorId, bool isOcuppied) {
            this.sensorId = sensorId;
            this.IsOccuppied = IsOccuppied;
        }
        public ParkingSensor(string sensorId) {
            this.sensorId = sensorId;
        }
    }
}
