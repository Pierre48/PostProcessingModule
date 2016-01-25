namespace PostProcessing
{
    /// <summary>
    /// Define what is a change on a text file
    /// </summary>
    public class Change
    {
        /// <summary>
        /// The string that must be replaced
        /// </summary>
        public string OldString { get; set; }
        /// <summary>
        /// The value that will be injected in the resposne
        /// </summary>
        public string NewString { get; set; }
    }
}