using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Line.Messaging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace A982078_linebot.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LineBotController : ControllerBase
    {
        private readonly HttpContext? _httpContext;
        private readonly LineBotConfig _lineBotConfig;

        public LineBotController(IServiceProvider serviceProvider)
        {
            var httpContextAccessor = serviceProvider.GetRequiredService<IHttpContextAccessor>();
            _httpContext = httpContextAccessor.HttpContext;
            _lineBotConfig = new LineBotConfig();
            _lineBotConfig.channelSecret = "9485f1110e17041495e11645217067c4";
            _lineBotConfig.accessToken = "mkR1uUX9AEiAGJzCnHwwgyNhUhLZB0J3fTOApG3wBHLIxniiQw8UWej+f20XAzbnrjgZxuwjLXHc60TLPXA8tXlsNUCNsGucIsALaMCzHRxo/UP589G3ikXik2fttsA16Zwyp6FPDaJY8nd8g4svCwdB04t89/1O/w1cDnyilFU=";
        }
        
        //完整的路由網址就是 https://xxx/api/linebot/run
        [HttpPost("run")]
        public async Task<IActionResult> Post()
        {
            try
            {
                if (_httpContext != null)
                {
                    var events = await _httpContext.Request.GetWebhookEventsAsync(_lineBotConfig.channelSecret);
                    var lineMessagingClient = new LineMessagingClient(_lineBotConfig.accessToken);
                    var lineBotApp = new LineBotApp(lineMessagingClient);
                    await lineBotApp.RunAsync(events);
                }
            }
            catch (Exception exp)
            {
                //需要 Log 可自行加入
                //_logger.LogError(JsonConvert.SerializeObject(ex));
            }
            return Ok();
        }
    }
}
