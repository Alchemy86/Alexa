using System;
using System.Collections.Generic;

namespace Cascade.Alexa.Core.Models
{
    public class AlexaResponse
    {
        public string Version { get; set; }

        public Attributes SessionAttributes { get; set; }

        public ResponseAttributes Response { get; set; }

        public AlexaResponse()
        {
            Version = "1.0";
            SessionAttributes = new Attributes
            {
                SessionId = null
            };
            Response = new ResponseAttributes();
        }

        public AlexaResponse(string outputSpeechText, bool shouldEndSession)
            : this()
        {
            Response.OutputSpeech.Ssml = outputSpeechText;
            Response.ShouldEndSession = shouldEndSession;

            if (shouldEndSession)
            {
                Response.Card.Content = outputSpeechText;
            }
            else
            {
                Response.Card = null;
            }
        }

        public AlexaResponse(string outputSpeechText, string cardContent)
            : this()
        {
            Response.OutputSpeech.Text = outputSpeechText;
            Response.Card.Content = cardContent;
        }

        public class Attributes
        {
            public string SessionId { get; set; }
        }

        public class ResponseAttributes
        {
            public bool ShouldEndSession { get; set; }

            public OutputSpeech OutputSpeech { get; set; }

            public Card Card { get; set; }

            public Reprompt Reprompt { get; set; }

            public ResponseAttributes()
            {
                ShouldEndSession = true;
                OutputSpeech = new OutputSpeech();
                Card = new Card();
                Reprompt = new Reprompt();
            }

        }

        public void HelpIntentHandler(Request request)
        {
            throw new NotImplementedException();
        }
    }


    public class OutputSpeech
    {
        public string Type { get; set; }

        public string Text { get; set; }

        public string Ssml { get; set; }

        public OutputSpeech()
        {
            Type = "SSML";
        }
    }

    public class Card
    {
        public string Type { get; set; }

        public string Title { get; set; }


        public string Content { get; set; }

        public Card()
        {
            Type = "Simple";
        }
    }

    public class Reprompt
    {
        public OutputSpeech OutputSpeech { get; set; }

        public Reprompt()
        {
            OutputSpeech = new OutputSpeech();
        }
    }
}
