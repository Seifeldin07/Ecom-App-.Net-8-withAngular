namespace Ecom.API.Helper
{
    public class ResponseAPI
    {
        public ResponseAPI(int statusCode, string message = null)
        {
            StatusCode = statusCode;
            Message = message?? GetMessageFromStatusCode(StatusCode);
        }

        private string GetMessageFromStatusCode(int StatusCode)
        {
            return StatusCode switch
            {
                200 => "Done",
                400 => "BadRequest",
                401 => "Un Athorized",
                404 => "Resource Not Found ",
                500 => "Server Erorr",
                _=>null,
            };
        }
        public int StatusCode { get; set; }
        public string? Message { get; set; }


    }
}
