using System.Diagnostics.CodeAnalysis;

namespace Common.Entities.Comparers
{
    public class ClientComparer : IEqualityComparer<Client>
    {
        public bool Equals(Client? x, Client? y)
        {
            if (x.Address.ToLower() == y.Address.ToLower() && x.Name.ToLower() == y.Name.ToLower())
            {
                return true;
            }

            return false;
        }

        public int GetHashCode([DisallowNull] Client obj)
        {
            return (obj.Address.ToLower() + obj.Name.ToLower()).GetHashCode();
        }
    }
}
