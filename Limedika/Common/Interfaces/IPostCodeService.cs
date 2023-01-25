namespace Common.Interfaces
{
    public interface IPostCodeService
    {
        /// <summary>
        /// Gets the postal code from a configured URI
        /// </summary>
        /// <param name="address">Address</param>
        /// <returns>A postal code</returns>
        Task<string> GetPostCode(string address);
    }
}
