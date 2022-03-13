////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

namespace DbcMetaLib.Confirmations
{
    public interface SavingChanges
    {
        public Task<int> SaveChangesAsync();
    }
}
