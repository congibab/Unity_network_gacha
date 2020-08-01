using System;

[Serializable]
public class sessionIDClass
{
    /// <summary>
    /// Session ID Class
    /// </summary>
    [Serializable]
    public class SessionID
    {
        public string session_id;
    }
    // Session ID
    public SessionID response;
    // 通信で得取するstatus
    public int status_code;

}
