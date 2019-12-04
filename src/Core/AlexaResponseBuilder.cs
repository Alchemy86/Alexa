using Cascade.Alexa.Core.Models;
using System.Text;

namespace Cascade.Alexa.Core
{
    public class AlexaResponseBuilder
    {
        private readonly StringBuilder _responseMessage;
        private readonly bool _endSession;
        private readonly string _cardTitle;
        private string _cardContent;
        private string _repromtMessage;

        public AlexaResponseBuilder(string cardTitle, string message, bool endSession)
        {
            _cardTitle = cardTitle;
            _cardContent = string.Empty;
            _responseMessage = new StringBuilder(message);
            _endSession = endSession;
        }

        public AlexaResponseBuilder SetCardContent(string cardContent)
        {
            _cardContent = cardContent;
            return this;
        }

        public AlexaResponseBuilder AppendSpeach(string message)
        {
            _responseMessage.Append(message);
            return this;
        }

        public AlexaResponseBuilder SetReprompt(string promptMessage)
        {
            _repromtMessage = _speak(promptMessage);
            return this;
        }

        public AlexaResponse Generate()
        {
            var response = new AlexaResponse(_speak(_responseMessage.ToString()), _endSession);
            response.Response.Card.Title = _cardTitle;
            response.Response.Card.Content = _cardContent;

            if (!string.IsNullOrEmpty(_repromtMessage))
                response.Response.Reprompt.OutputSpeech.Ssml = _repromtMessage;

            return response;
        }

        private string _speak(string message)
        {
            return $"<speak>{message}</speak>";
        }
    }
}
