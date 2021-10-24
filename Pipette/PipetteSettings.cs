using Newtonsoft.Json;
using PeterHan.PLib.Options;

namespace Pipette
{
    [JsonObject(MemberSerialization.OptIn)]
    public class PipetteSettings
    {
        [Option("Capacity (kg)", "The capacity (kg) of pipette")]
        [Limit(0.01f, 0.1f)]
        [JsonProperty]
        public float capacity { get; set; }

        public PipetteSettings()
        {
            this.capacity = 0.01f;
        }
    }
}