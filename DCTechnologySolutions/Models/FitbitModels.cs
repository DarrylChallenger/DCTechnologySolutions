using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace DCTechnologySolutions.Models
{
    public class FitbitResponseModel
    {
        public FitbitResponseModel()
        {
            isSuccessStatsCode = false;
            successPayload = null;
            activityLifetimePayload = null;
            apiError = null;
        }
        public bool isSuccessStatsCode { get; set; }
        public string errorMessage { get; set; }
        public HttpStatusCode statusCode { get; set; }
        public string accessToken { get; set; }
        public string resrtToken { get; set; }
        public apiErrorPayload apiError { get; set; }
        public authErrorPayload payloadEerror { get; set; }
        public authSuccessPayload successPayload { get; set; }
        public activityLifetimePayload activityLifetimePayload { get; set; }
    }

    public class authError
    {
        public string errorType { get; set; }
        public string message { get; set; }
    }

    public class authErrorPayload
    {
        public List<authError> errors { get; set; }
        public string success { get; set; }
    }

    public class authSuccessPayload
    {
        public string access_token { get; set; }
        public string expires_in { get; set; }
        public string refresh_token { get; set; }
        public string scope { get; set; }
        public string token_type { get; set; }
        public string user_id { get; set; }
    }

    public class apiError
    {
        public string errorType { get; set; }
        public string message { get; set; }
    }

    public class apiErrorPayload
    {
        public List<authError> errors { get; set; }
        public string success { get; set; }
    }

    public class activityLifeTimeStats
    {
        public string activeScore { get; set; }
        public string caloriesOut { get; set; }
        public string distance { get; set; }
        public string steps { get; set; }
    }

    public class activityLifetime
    {
        public activityLifeTimeStats total { get; set; }
        public activityLifeTimeStats tracker { get; set; }
    }
    public class activityLifetimePayload
    {
        public activityLifetime lifetime { get; set; }
    }    //"{\"lifetime\":{\"total\":{\"activeScore\":-1,\"caloriesOut\":-1,\"distance\":0,\"steps\":0},\"tracker\":{\"activeScore\":-1,\"caloriesOut\":-1,\"distance\":0,\"steps\":0}}}"
}