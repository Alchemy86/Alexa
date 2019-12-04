using System;
using System.Collections.Generic;

namespace Cascade.Alexa.Core.Models
{
    public class Application
    {
        public string ApplicationId { get; set; }
    }

    public class Permissions
    {
        public string ConsentToken { get; set; }
    }

    public class User
    {
        public string UserId { get; set; }
        public Permissions Permissions { get; set; }
    }

    public class Session
    {
        public bool @new { get; set; }
        public string SessionId { get; set; }
        public Application Application { get; set; }
        public User User { get; set; }
    }

    public class Application2
    {
        public string ApplicationId { get; set; }
    }

    public class Permissions2
    {
        public string ConsentToken { get; set; }
    }

    public class User2
    {
        public string UserId { get; set; }
        public Permissions2 Permissions { get; set; }
    }

    public class SupportedInterfaces
    {
    }

    public class Device
    {
        public string DeviceId { get; set; }
        public SupportedInterfaces SupportedInterfaces { get; set; }
    }

    public class System
    {
        public Application2 Application { get; set; }
        public User2 User { get; set; }
        public Device Device { get; set; }
        public string ApiEndpoint { get; set; }
        public string ApiAccessToken { get; set; }
    }

    public class Experience
    {
        public int ArcMinuteWidth { get; set; }
        public int ArcMinuteHeight { get; set; }
        public bool CanRotate { get; set; }
        public bool CanResize { get; set; }
    }

    public class Video
    {
        public List<string> Codecs { get; set; }
    }

    public class Viewport
    {
        public List<Experience> Experiences { get; set; }
        public string Shape { get; set; }
        public int PixelWidth { get; set; }
        public int PixelHeight { get; set; }
        public int Dpi { get; set; }
        public int CurrentPixelWidth { get; set; }
        public int CurrentPixelHeight { get; set; }
        public List<string> Touch { get; set; }
        public Video Video { get; set; }
    }

    public class Context
    {
        public System System { get; set; }
        public Viewport Viewport { get; set; }
    }

    /// <summary>
    /// Area of logic and conversation identified by Alexa based on user input
    /// </summary>
    public class Intent
    {
        public string Name { get; set; }
        public string ConfirmationStatus { get; set; }
        public dynamic Slots { get; set; }
        public List<KeyValuePair<string, string>> GetSlots()
        {
            var output = new List<KeyValuePair<string, string>>();
            if (Slots == null) return output;

            foreach (var slot in Slots.Children())
            {
                if (slot.First.value != null)
                    output.Add(new KeyValuePair<string, string>(slot.First.name.ToString(), slot.First.value.ToString()));
            }

            return output;
        }
    }

    /// <summary>
    /// Slots - Additioanal intent information for dynamic user data
    /// https://developer.amazon.com/docs/custom-skills/slot-type-reference.html
    /// </summary>
    public class Slot
    {
        public string Name { get; set; }
        public string ConfirmationStatus { get; set; }
        public string Value { get; set; }
        public string Source { get; set; }
    }

    public class Request
    {
        public string Type { get; set; }
        public string RequestId { get; set; }
        public DateTime Timestamp { get; set; }
        public string Locale { get; set; }
        public Intent Intent { get; set; }
    }

    public class AlexaRequest
    {
        public string Version { get; set; }
        public Session Session { get; set; }
        public Context Context { get; set; }
        public Request Request { get; set; }
    }
}
