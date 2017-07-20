using SPAccounts.DataAccessObject.DTO;
using SPAccounts.RepositoryServices.Contracts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace SPAccounts.RepositoryServices.Services
{
    public class FileUploadRepository:IFileUploadRepository
    {
        private IDatabaseFactory _databaseFactory;
        public FileUploadRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }
        public FileUpload InsertAttachment(FileUpload fileUploadObj)
        {
            try
            {
                SqlParameter outputStatus, outputParentID, outputID = null;
                using (SqlConnection con = _databaseFactory.GetDBConnection())
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        if (con.State == ConnectionState.Closed)
                        {
                            con.Open();
                        }
                        cmd.Connection = con;
                        cmd.CommandText = "[Accounts].[InsertAttachment]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ParentID", SqlDbType.UniqueIdentifier).Value = fileUploadObj.ParentID;
                        cmd.Parameters.Add("@ParentType", SqlDbType.VarChar, 20).Value = fileUploadObj.ParentType;
                        cmd.Parameters.Add("@FileName", SqlDbType.NVarChar, 255).Value = fileUploadObj.FileName;
                        cmd.Parameters.Add("@FileType", SqlDbType.VarChar, 5).Value = fileUploadObj.FileType;
                        cmd.Parameters.Add("@FileSize", SqlDbType.VarChar, 50).Value = fileUploadObj.FileSize;
                        cmd.Parameters.Add("@AttachmentURL", SqlDbType.NVarChar, -1).Value = fileUploadObj.AttachmentURL;
                        cmd.Parameters.Add("@CreatedBy", SqlDbType.NVarChar, 250).Value = fileUploadObj.commonObj.CreatedBy;
                        cmd.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = fileUploadObj.commonObj.CreatedDate;
                        outputStatus = cmd.Parameters.Add("@Status", SqlDbType.SmallInt);
                        outputStatus.Direction = ParameterDirection.Output;
                        outputID = cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier);
                        outputID.Direction = ParameterDirection.Output;
                        outputParentID = cmd.Parameters.Add("@ParentID", SqlDbType.UniqueIdentifier);
                        outputParentID.Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();


                    }
                }

                switch (outputStatus.Value.ToString())
                {
                    case "0":
                        AppConst Cobj = new AppConst();
                        throw new Exception(Cobj.InsertFailure);
                    case "1":
                        fileUploadObj.ID = Guid.Parse(outputID.Value.ToString());
                        fileUploadObj.ParentID = Guid.Parse(outputParentID.Value.ToString());
                        break;
                    case "2":
                        AppConst Cobj1 = new AppConst();
                        throw new Exception(Cobj1.Duplicate);
                    default:
                        break;
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
            return fileUploadObj;
        }
    }
}