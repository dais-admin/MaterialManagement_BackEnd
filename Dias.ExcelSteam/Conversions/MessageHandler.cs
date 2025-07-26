using Dias.ExcelSteam.Dtos;
using Newtonsoft.Json;

namespace Dias.ExcelSteam.Conversions
{
    public class MessageHandler : IMessageHandler
    {
        public async Task<MaterialDto> DeserializeMessage(string message)
        {

            var productDto = JsonConvert.DeserializeObject<RootDto>(message);
            var result = await Task.Run(() => productDto);
            return result!.Asset;
        }
    }
}
