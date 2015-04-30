namespace DALSample
{
    public class HumanRepo : Repo<Human>, IHumanRepo
    {
        public HumanRepo(IConnectionFactory connectionFactory) : base(connectionFactory)
        {
        }
        
        /// <summary> 
        /// this doesn't work because there is no assignRole SP in the Db
        /// just to show the thing that you can execute sp with parameters
        /// </summary>
        public int Assign(int id, int roleId)
        {
            return DbUtil.ExecuteNonQuerySp("assignRole", Cs, new { id, roleid = roleId});
        }

        public void DeleteAll()
        {
            DbUtil.ExecuteNonQuerySp("deleteAll", Cs, null);
        }
    }

    public interface IHumanRepo : IRepo<Human>
    {
        int Assign(int id, int roleId);
        void DeleteAll();
    }
}