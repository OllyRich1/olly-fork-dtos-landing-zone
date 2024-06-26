using System.Net;
using System.Text;
using System.Text.Json;
using Common;
using Data.Database;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Model;

namespace markParticipantAsEligible
{
    public class MarkParticipantAsEligible
    {
        private readonly ILogger<MarkParticipantAsEligible> _logger;
        private readonly ICreateResponse _createResponse;
        private readonly IUpdateParticipantData _updateParticipantData;

        public MarkParticipantAsEligible(ILogger<MarkParticipantAsEligible> logger, ICreateResponse createResponse, IUpdateParticipantData updateParticipant)
        {
            _logger = logger;
            _createResponse = createResponse;
            _updateParticipantData = updateParticipant;
        }

        [Function("markParticipantAsEligible")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req)
        {
            // convert body to json and then deserialize to object
            string postdata = "";
            using (StreamReader reader = new StreamReader(req.Body, Encoding.UTF8))
            {
                postdata = await reader.ReadToEndAsync();
            }

            var participant = JsonSerializer.Deserialize<Participant>(postdata);

            try
            {
                var updated = false;
                if (participant != null)
                {
                    updated = _updateParticipantData.UpdateParticipantAsEligible(participant, 'Y');

                }
                if (updated)
                {
                    _logger.LogInformation($"record updated for participant {participant.NHSId}");
                    return _createResponse.CreateHttpResponse(HttpStatusCode.OK, req);
                }

                _logger.LogError($"an error occurred while updating data for {participant.NHSId}");
                return _createResponse.CreateHttpResponse(HttpStatusCode.BadRequest, req);

            }
            catch (Exception ex)
            {
                _logger.LogError($"an error occurred: {ex}");
                return _createResponse.CreateHttpResponse(HttpStatusCode.BadRequest, req);
            }

        }
    }
}
