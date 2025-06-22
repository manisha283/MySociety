// using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.AspNetCore.SignalR;
// using MySociety.Web.Hubs;
// using System.Security.Claims;

// namespace MySociety.Web.Controllers
// {
//     [Authorize]
//     [Route("api/[controller]")]
//     [ApiController]
//     public class NotificationController : ControllerBase
//     {
//         private readonly IHubContext<NotificationHub> _hubContext;

//         public NotificationController(IHubContext<NotificationHub> hubContext)
//         {
//             _hubContext = hubContext;
//         }

//         [HttpPost("send")]
//         public async Task<IActionResult> SendNotification([FromBody] NotificationRequest request)
//         {
//             if (string.IsNullOrEmpty(request.TargetUserId) || string.IsNullOrEmpty(request.Message))
//             {
//                 return BadRequest("Target user ID and message are required.");
//             }

//             // Send notification to specific user
//             await _hubContext.Clients.User(request.TargetUserId).SendAsync(
//                 "ReceiveNotification",
//                 User.FindFirst(ClaimTypes.NameIdentifier)?.Value,
//                 request.Message
//             );

//             return Ok("Notification sent.");
//         }
//     }

//     public class NotificationRequest
//     {
//         public string TargetUserId { get; set; }
//         public string Message { get; set; }
//     }
// }