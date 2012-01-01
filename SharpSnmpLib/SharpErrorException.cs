/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2008/4/23
 * Time: 19:40
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Globalization;
using System.Net;
#if (!SILVERLIGHT)
using System.Runtime.Serialization;
using System.Security.Permissions; 
#endif

namespace Lextm.SharpSnmpLib
{
    /// <summary>
    /// Error exception of #SNMP. Raised when an error message is received.
    /// </summary>
    [Serializable]
    public sealed class SharpErrorException : SharpOperationException
    {
        private GetResponseMessage _body;
        
        /// <summary>
        /// Message body.
        /// </summary>
        public GetResponseMessage Body
        {
            get { return _body; }
        }        
        
        /// <summary>
        /// Creates a <see cref="SharpErrorException"/> instance.
        /// </summary>
        public SharpErrorException()
        {
        }
        
        /// <summary>
        /// Creates a <see cref="SharpErrorException"/> instance with a specific <see cref="string"/>.
        /// </summary>
        /// <param name="message">Message</param>
        public SharpErrorException(string message) : base(message)
        {
        }
        
        /// <summary>
        /// Creates a <see cref="SharpErrorException"/> instance with a specific <see cref="string"/> and an <see cref="Exception"/>.
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="inner">Inner exception</param>
        public SharpErrorException(string message, Exception inner)
            : base(message, inner)
        {
        }
#if (!SILVERLIGHT)
        private SharpErrorException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            if (info == null)
            {
                throw new ArgumentNullException("info");
            }
            
            _body = (GetResponseMessage)info.GetValue("Body", typeof(GetResponseMessage));
        }
        
        /// <summary>
        /// Gets object data.
        /// </summary>
        /// <param name="info">Info</param>
        /// <param name="context">Context</param>
        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("Body", _body);
        }
#endif
        /// <summary>
        /// Details on error.
        /// </summary>
        public override string Details
        {
            get
            {
                return string.Format(
                    CultureInfo.InvariantCulture,
                    "{0}. {1}. Index: {2}. Errored Object ID: {3}",
                    Message,
                    Body.ErrorStatus,
                    Body.ErrorIndex.ToString(CultureInfo.InvariantCulture),
                    Body.ErrorIndex == 0 ? null : Body.Variables[Body.ErrorIndex - 1].Id);
            }
        }
        
        /// <summary>
        /// Returns a <see cref="String"/> that represents this <see cref="SharpErrorException"/>.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "SharpErrorException: " + Details;
        }
        
        /// <summary>
        /// Creates a <see cref="SharpErrorException"/>.
        /// </summary>
        /// <param name="message">Message.</param>
        /// <param name="agent">Agent address.</param>
        /// <param name="body">Error message body.</param>
            /// <returns></returns>
        public static SharpErrorException Create(string message, IPAddress agent, GetResponseMessage body)
        {
            SharpErrorException ex = new SharpErrorException(message);
            ex.Agent = agent;
            ex._body = body;
            return ex;
        }
    }
}