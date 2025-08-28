using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Transactions;
using System.Web.Http;
using UserSyncApi.Authentication;
using UserSyncApi.Helpers;
using UserSyncApi.Models;

namespace UserSyncApi.Controllers
{
    [BasicAuthentication]
    public class UsersController : ApiController
    {
        [HttpPost]
        [Route("api/users/create")]
        public IHttpActionResult CreateUser([FromBody] CreateUserRequest request)
        {
            try
            {
                using (var scope = new TransactionScope())
                {

                    foreach (var dbKey in request.TargetDatabases)
                    {

                        using (SqlConnection conn = DbConnectionFactory.GetConnection(dbKey))
                        {
                            conn.Open();

                            string sql = @"
                INSERT INTO Users (
                    user_name, user_loginname, user_loggedonat, user_fullname, user_email,
                    user_initials, user_password, user_department, user_loggedin, user_inmodule,
                    g2version, lastlogin, os, clr, user_menus, logincount, libraries_readonly,
                    group_company, screen1_res, screen2_res, screen3_res, screen4_res,
                    po_auth_id, po_auth_all, order_alerts, po_auth_temp_user_id, maxordervalue,
                    outofoffice, default_order_department, porole_id, deleted, alias_username_1,
                    invoicebarcodeprinter, smtpserver, factory_id, piecemonitoringaccesslevel,
                    exclassedit, timesheetsaccesslevel, initial_windows, fabscheduleaccesslevel,
                    fablinescheduleaccesslevel, paintlinescheduleaccesslevel, contractsaccesslevel,
                    g2updaterversion, updatelocation_id, allocationadmin, user_password_last_changed,
                    date_created, createdbyuser_id, date_modified, modifiedbyuser_id,
                    loggedinoncomputer, yloc, ylocdsc, locked, fattempt, remarks, releasedt,
                    releaseby, inactive, inactiveremarks, inactivereleasedt, inactivereleaseby,
                    password_attempts, password_updated_flag, unlock_date
                )
                VALUES (
                    @UserName, @UserLoginName, @UserLoggedOnAt, @UserFullName, @UserEmail,
                    @UserInitials, @UserPassword, @UserDepartment, @UserLoggedIn, @UserInModule,
                    @G2Version, @LastLogin, @OS, @CLR, @UserMenus, @LoginCount, @LibrariesReadOnly,
                    @GroupCompany, @Screen1Res, @Screen2Res, @Screen3Res, @Screen4Res,
                    @POAuthId, @POAuthAll, @OrderAlerts, @POAuthTempUserId, @MaxOrderValue,
                    @OutOfOffice, @DefaultOrderDepartment, @PORoleId, @Deleted, @AliasUserName1,
                    @InvoiceBarcodePrinter, @SmtpServer, @FactoryId, @PieceMonitoringAccessLevel,
                    @ExClassEdit, @TimesheetsAccessLevel, @InitialWindows, @FabScheduleAccessLevel,
                    @FabLineScheduleAccessLevel, @PaintLineScheduleAccessLevel, @ContractsAccessLevel,
                    @G2UpdaterVersion, @UpdateLocationId, @AllocationAdmin, @UserPasswordLastChanged,
                    @DateCreated, @CreatedByUserId, @DateModified, @ModifiedByUserId,
                    @LoggedInOnComputer, @Yloc, @YlocDsc, @Locked, @FAttempt, @Remarks, @ReleaseDt,
                    @ReleaseBy, @Inactive, @InactiveRemarks, @InactiveReleaseDt, @InactiveReleaseBy,
                    @PasswordAttempts, @PasswordUpdatedFlag, @UnlockDate
                );
                SELECT SCOPE_IDENTITY();";

                            using (SqlCommand cmd = new SqlCommand(sql, conn))
                            {
                                cmd.Parameters.AddWithValue("@UserName", request.UserName);
                                cmd.Parameters.AddWithValue("@UserLoginName", request.UserLoginName);
                                cmd.Parameters.AddWithValue("@UserLoggedOnAt", request.UserLoggedOnAt);
                                cmd.Parameters.AddWithValue("@UserFullName", request.UserFullName);
                                cmd.Parameters.AddWithValue("@UserEmail", request.UserEmail);
                                cmd.Parameters.AddWithValue("@UserInitials", request.UserInitials);
                                cmd.Parameters.AddWithValue("@UserPassword", request.UserPassword);
                                cmd.Parameters.AddWithValue("@UserDepartment", request.UserDepartment);
                                cmd.Parameters.AddWithValue("@UserLoggedIn", request.UserLoggedIn);
                                cmd.Parameters.AddWithValue("@UserInModule", request.UserInModule);
                                cmd.Parameters.AddWithValue("@G2Version", (object)request.G2Version ?? DBNull.Value);
                                cmd.Parameters.AddWithValue("@LastLogin", (object)request.LastLogin ?? DBNull.Value);
                                cmd.Parameters.AddWithValue("@OS", request.OS);
                                cmd.Parameters.AddWithValue("@CLR", request.CLR);
                                cmd.Parameters.AddWithValue("@UserMenus", request.UserMenus);
                                cmd.Parameters.AddWithValue("@LoginCount", request.LoginCount);
                                cmd.Parameters.AddWithValue("@LibrariesReadOnly", request.LibrariesReadOnly);
                                cmd.Parameters.AddWithValue("@GroupCompany", request.GroupCompany);
                                cmd.Parameters.AddWithValue("@Screen1Res", request.Screen1Res);
                                cmd.Parameters.AddWithValue("@Screen2Res", request.Screen2Res);
                                cmd.Parameters.AddWithValue("@Screen3Res", request.Screen3Res);
                                cmd.Parameters.AddWithValue("@Screen4Res", request.Screen4Res);
                                cmd.Parameters.AddWithValue("@POAuthId", request.POAuthId);
                                cmd.Parameters.AddWithValue("@POAuthAll", request.POAuthAll);
                                cmd.Parameters.AddWithValue("@OrderAlerts", request.OrderAlerts);
                                cmd.Parameters.AddWithValue("@POAuthTempUserId", request.POAuthTempUserId);
                                cmd.Parameters.AddWithValue("@MaxOrderValue", request.MaxOrderValue);
                                cmd.Parameters.AddWithValue("@OutOfOffice", request.OutOfOffice);
                                cmd.Parameters.AddWithValue("@DefaultOrderDepartment", request.DefaultOrderDepartment);
                                cmd.Parameters.AddWithValue("@PORoleId", request.PORoleId);
                                cmd.Parameters.AddWithValue("@Deleted", request.Deleted);
                                cmd.Parameters.AddWithValue("@AliasUserName1", (object)request.AliasUserName1 ?? DBNull.Value);
                                cmd.Parameters.AddWithValue("@InvoiceBarcodePrinter", request.InvoiceBarcodePrinter);
                                cmd.Parameters.AddWithValue("@SmtpServer", request.SmtpServer);
                                cmd.Parameters.AddWithValue("@FactoryId", request.FactoryId);
                                cmd.Parameters.AddWithValue("@PieceMonitoringAccessLevel", request.PieceMonitoringAccessLevel);
                                cmd.Parameters.AddWithValue("@ExClassEdit", request.ExClassEdit);
                                cmd.Parameters.AddWithValue("@TimesheetsAccessLevel", request.TimesheetsAccessLevel);
                                cmd.Parameters.AddWithValue("@InitialWindows", request.InitialWindows);
                                cmd.Parameters.AddWithValue("@FabScheduleAccessLevel", request.FabScheduleAccessLevel);
                                cmd.Parameters.AddWithValue("@FabLineScheduleAccessLevel", request.FabLineScheduleAccessLevel);
                                cmd.Parameters.AddWithValue("@PaintLineScheduleAccessLevel", request.PaintLineScheduleAccessLevel);
                                cmd.Parameters.AddWithValue("@ContractsAccessLevel", request.ContractsAccessLevel);
                                cmd.Parameters.AddWithValue("@G2UpdaterVersion", (object)request.G2UpdaterVersion ?? DBNull.Value);
                                cmd.Parameters.AddWithValue("@UpdateLocationId", request.UpdateLocationId);
                                cmd.Parameters.AddWithValue("@AllocationAdmin", request.AllocationAdmin);
                                cmd.Parameters.AddWithValue("@UserPasswordLastChanged", (object)request.UserPasswordLastChanged ?? DBNull.Value);
                                cmd.Parameters.AddWithValue("@DateCreated", (object)request.DateCreated ?? DBNull.Value);
                                cmd.Parameters.AddWithValue("@CreatedByUserId", (object)request.CreatedByUserId ?? DBNull.Value);
                                cmd.Parameters.AddWithValue("@DateModified", (object)request.DateModified ?? DBNull.Value);
                                cmd.Parameters.AddWithValue("@ModifiedByUserId", (object)request.ModifiedByUserId ?? DBNull.Value);
                                cmd.Parameters.AddWithValue("@LoggedInOnComputer", request.LoggedInOnComputer);
                                cmd.Parameters.AddWithValue("@Yloc", (object)request.YLoc ?? DBNull.Value);
                                cmd.Parameters.AddWithValue("@YlocDsc", (object)request.YLocDsc ?? DBNull.Value);
                                cmd.Parameters.AddWithValue("@Locked", (object)request.Locked ?? DBNull.Value);
                                cmd.Parameters.AddWithValue("@FAttempt", (object)request.FAttempt ?? DBNull.Value);
                                cmd.Parameters.AddWithValue("@Remarks", (object)request.Remarks ?? DBNull.Value);
                                cmd.Parameters.AddWithValue("@ReleaseDt", (object)request.ReleaseDt ?? DBNull.Value);
                                cmd.Parameters.AddWithValue("@ReleaseBy", (object)request.ReleaseBy ?? DBNull.Value);
                                cmd.Parameters.AddWithValue("@Inactive", (object)request.Inactive ?? DBNull.Value);
                                cmd.Parameters.AddWithValue("@InactiveRemarks", (object)request.InactiveRemarks ?? DBNull.Value);
                                cmd.Parameters.AddWithValue("@InactiveReleaseDt", (object)request.InactiveReleaseDt ?? DBNull.Value);
                                cmd.Parameters.AddWithValue("@InactiveReleaseBy", (object)request.InactiveReleaseBy ?? DBNull.Value);
                                cmd.Parameters.AddWithValue("@PasswordAttempts", (object)request.PasswordAttempts ?? DBNull.Value);
                                cmd.Parameters.AddWithValue("@PasswordUpdatedFlag", (object)request.PasswordUpdatedFlag ?? DBNull.Value);
                                cmd.Parameters.AddWithValue("@UnlockDate", (object)request.UnlockDate ?? DBNull.Value);

                                var newId = Convert.ToInt32(cmd.ExecuteScalar());
                            }
                        }
                    }
                }
                return Content(HttpStatusCode.OK, Common.Common.Messages.USER_CREATED_SUCCESSFULLY);
            }
            catch (Exception ex)
            {
                Logger.Log($"Error in CreateUser: {ex.Message}");
                Logger.Log($"StackTrace in CreateUser: { ex.StackTrace.ToString()}");
                return Content(HttpStatusCode.InternalServerError, Common.Common.Messages.AN_UNEXPECTED_ERROR_OCCURRED);
            }
        }

        [HttpGet]
        [Route("api/users/get")]
        public IHttpActionResult GetUser(int UserId)
        {
            try
            {
                var users = new List<dynamic>
                        {
                            new { Id = 1, Name = "John Doe", Email = "john@example.com", Age = 30 },
                            new { Id = 2, Name = "Jane Smith", Email = "jane@example.com", Age = 28 }
                        };
                if (UserId <= 0)
                {
                    return BadRequest($"{UserId} is {Common.Common.Messages.INVALID_USER_ID}"); // Filter wraps this
                }

                var user = users.FirstOrDefault(u => u.Id == UserId);

                if (user == null)
                {
                    return Content(HttpStatusCode.NotFound, new { Message = $"User with Id {UserId} was not found." });
                }

                return Ok(user);

            }
            catch (Exception ex)
            {
                Logger.Log($"Error in GetUser: { ex.Message}");
                Logger.Log($"StackTrace in GetUser: { ex.StackTrace.ToString()}");
                return Content(HttpStatusCode.InternalServerError, new { Message = Common.Common.Messages.AN_UNEXPECTED_ERROR_OCCURRED });
            }

        }

        [HttpGet]
        [Route("api/users/getAll")]
        public IHttpActionResult GetAllUser()
        {
            // sync logic here
            return Ok("Users synchronized successfully");
        }

        [HttpPut]
        [Route("api/users/update")]
        public IHttpActionResult UpdateUser([FromBody] UpdateUserRequest request)
        {
            try
            {
                using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    foreach (var dbKey in request.TargetDatabases)
                    {
                        using (SqlConnection conn = DbConnectionFactory.GetConnection(dbKey))
                        {
                            conn.Open();
                            string sql = @"
                                        UPDATE Users SET
                                            user_name=@UserName,
                                            user_loginname=@UserLoginName,
                                            user_loggedonat=@UserLoggedOnAt,
                                            user_fullname=@UserFullName,
                                            user_email=@UserEmail,
                                            user_initials=@UserInitials,
                                            user_password=@UserPassword,
                                            user_department=@UserDepartment,
                                            user_loggedin=@UserLoggedIn,
                                            user_inmodule=@UserInModule,
                                            g2version=@G2Version,
                                            lastlogin=@LastLogin,
                                            os=@OS,
                                            clr=@CLR,
                                            user_menus=@UserMenus,
                                            logincount=@LoginCount,
                                            libraries_readonly=@LibrariesReadonly,
                                            group_company=@GroupCompany,
                                            screen1_res=@Screen1Res,
                                            screen2_res=@Screen2Res,
                                            screen3_res=@Screen3Res,
                                            screen4_res=@Screen4Res,
                                            po_auth_id=@PoAuthId,
                                            po_auth_all=@PoAuthAll,
                                            order_alerts=@OrderAlerts,
                                            po_auth_temp_user_id=@PoAuthTempUserId,
                                            maxordervalue=@MaxOrderValue,
                                            outofoffice=@OutOfOffice,
                                            default_order_department=@DefaultOrderDepartment,
                                            porole_id=@PoRoleId,
                                            deleted=@Deleted,
                                            alias_username_1=@AliasUserName1,
                                            invoicebarcodeprinter=@InvoiceBarcodePrinter,
                                            smtpserver=@SmtpServer,
                                            factory_id=@FactoryId,
                                            piecemonitoringaccesslevel=@PieceMonitoringAccessLevel,
                                            exclassedit=@ExClassEdit,
                                            timesheetsaccesslevel=@TimesheetsAccessLevel,
                                            initial_windows=@InitialWindows,
                                            fabscheduleaccesslevel=@FabScheduleAccessLevel,
                                            fablinescheduleaccesslevel=@FabLineScheduleAccessLevel,
                                            paintlinescheduleaccesslevel=@PaintLineScheduleAccessLevel,
                                            contractsaccesslevel=@ContractsAccessLevel,
                                            g2updaterversion=@G2UpdaterVersion,
                                            updatelocation_id=@UpdateLocationId,
                                            allocationadmin=@AllocationAdmin,
                                            user_password_last_changed=@UserPasswordLastChanged,
                                            date_created=@DateCreated,
                                            createdbyuser_id=@CreatedByUserId,
                                            date_modified=@DateModified,
                                            modifiedbyuser_id=@ModifiedByUserId,
                                            loggedinoncomputer=@LoggedInOnComputer,
                                            yloc=@YLoc,
                                            ylocdsc=@YLocDsc,
                                            locked=@Locked,
                                            fattempt=@FAttempt,
                                            remarks=@Remarks,
                                            releasedt=@ReleaseDt,
                                            releaseby=@ReleaseBy,
                                            inactive=@Inactive,
                                            inactiveremarks=@InactiveRemarks,
                                            inactivereleasedt=@InactiveReleaseDt,
                                            inactivereleaseby=@InactiveReleaseBy,
                                            password_attempts=@PasswordAttempts,
                                            password_updated_flag=@PasswordUpdatedFlag,
                                            unlock_date=@UnlockDate
                                        WHERE user_id=@UserId";

                            using (SqlCommand cmd = new SqlCommand(sql, conn))
                            {
                                // Parameters (mapping request properties)
                                cmd.Parameters.AddWithValue("@UserId", request.UserId);
                                cmd.Parameters.AddWithValue("@UserName", request.UserName);
                                cmd.Parameters.AddWithValue("@UserLoginName", request.UserLoginName);
                                cmd.Parameters.AddWithValue("@UserLoggedOnAt", request.UserLoggedOnAt);
                                cmd.Parameters.AddWithValue("@UserFullName", request.UserFullName);
                                cmd.Parameters.AddWithValue("@UserEmail", request.UserEmail);
                                cmd.Parameters.AddWithValue("@UserInitials", request.UserInitials);
                                cmd.Parameters.AddWithValue("@UserPassword", request.UserPassword);
                                cmd.Parameters.AddWithValue("@UserDepartment", request.UserDepartment);
                                cmd.Parameters.AddWithValue("@UserLoggedIn", request.UserLoggedIn);
                                cmd.Parameters.AddWithValue("@UserInModule", request.UserInModule);
                                cmd.Parameters.AddWithValue("@G2Version", (object)request.G2Version ?? DBNull.Value);
                                cmd.Parameters.AddWithValue("@LastLogin", (object)request.LastLogin ?? DBNull.Value);
                                cmd.Parameters.AddWithValue("@OS", request.OS);
                                cmd.Parameters.AddWithValue("@CLR", request.CLR);
                                cmd.Parameters.AddWithValue("@UserMenus", request.UserMenus);
                                cmd.Parameters.AddWithValue("@LoginCount", request.LoginCount);
                                cmd.Parameters.AddWithValue("@LibrariesReadonly", request.LibrariesReadOnly);
                                cmd.Parameters.AddWithValue("@GroupCompany", request.GroupCompany);
                                cmd.Parameters.AddWithValue("@Screen1Res", request.Screen1Res);
                                cmd.Parameters.AddWithValue("@Screen2Res", request.Screen2Res);
                                cmd.Parameters.AddWithValue("@Screen3Res", request.Screen3Res);
                                cmd.Parameters.AddWithValue("@Screen4Res", request.Screen4Res);
                                cmd.Parameters.AddWithValue("@PoAuthId", request.POAuthId);
                                cmd.Parameters.AddWithValue("@PoAuthAll", request.POAuthAll);
                                cmd.Parameters.AddWithValue("@OrderAlerts", request.OrderAlerts);
                                cmd.Parameters.AddWithValue("@PoAuthTempUserId", request.POAuthTempUserId);
                                cmd.Parameters.AddWithValue("@MaxOrderValue", request.MaxOrderValue);
                                cmd.Parameters.AddWithValue("@OutOfOffice", request.OutOfOffice);
                                cmd.Parameters.AddWithValue("@DefaultOrderDepartment", request.DefaultOrderDepartment);
                                cmd.Parameters.AddWithValue("@PoRoleId", request.PORoleId);
                                cmd.Parameters.AddWithValue("@Deleted", request.Deleted);
                                cmd.Parameters.AddWithValue("@AliasUserName1", (object)request.AliasUserName1 ?? DBNull.Value);
                                cmd.Parameters.AddWithValue("@InvoiceBarcodePrinter", request.InvoiceBarcodePrinter);
                                cmd.Parameters.AddWithValue("@SmtpServer", request.SmtpServer);
                                cmd.Parameters.AddWithValue("@FactoryId", request.FactoryId);
                                cmd.Parameters.AddWithValue("@PieceMonitoringAccessLevel", request.PieceMonitoringAccessLevel);
                                cmd.Parameters.AddWithValue("@ExClassEdit", request.ExClassEdit);
                                cmd.Parameters.AddWithValue("@TimesheetsAccessLevel", request.TimesheetsAccessLevel);
                                cmd.Parameters.AddWithValue("@InitialWindows", request.InitialWindows);
                                cmd.Parameters.AddWithValue("@FabScheduleAccessLevel", request.FabScheduleAccessLevel);
                                cmd.Parameters.AddWithValue("@FabLineScheduleAccessLevel", request.FabLineScheduleAccessLevel);
                                cmd.Parameters.AddWithValue("@PaintLineScheduleAccessLevel", request.PaintLineScheduleAccessLevel);
                                cmd.Parameters.AddWithValue("@ContractsAccessLevel", request.ContractsAccessLevel);
                                cmd.Parameters.AddWithValue("@G2UpdaterVersion", (object)request.G2UpdaterVersion ?? DBNull.Value);
                                cmd.Parameters.AddWithValue("@UpdateLocationId", request.UpdateLocationId);
                                cmd.Parameters.AddWithValue("@AllocationAdmin", request.AllocationAdmin);
                                cmd.Parameters.AddWithValue("@UserPasswordLastChanged", (object)request.UserPasswordLastChanged ?? DBNull.Value);
                                cmd.Parameters.AddWithValue("@DateCreated", (object)request.DateCreated ?? DBNull.Value);
                                cmd.Parameters.AddWithValue("@CreatedByUserId", (object)request.CreatedByUserId ?? DBNull.Value);
                                cmd.Parameters.AddWithValue("@DateModified", (object)request.DateModified ?? DBNull.Value);
                                cmd.Parameters.AddWithValue("@ModifiedByUserId", (object)request.ModifiedByUserId ?? DBNull.Value);
                                cmd.Parameters.AddWithValue("@LoggedInOnComputer", request.LoggedInOnComputer);
                                cmd.Parameters.AddWithValue("@YLoc", (object)request.YLoc ?? DBNull.Value);
                                cmd.Parameters.AddWithValue("@YLocDsc", (object)request.YLocDsc ?? DBNull.Value);
                                cmd.Parameters.AddWithValue("@Locked", (object)request.Locked ?? DBNull.Value);
                                cmd.Parameters.AddWithValue("@FAttempt", (object)request.FAttempt ?? DBNull.Value);
                                cmd.Parameters.AddWithValue("@Remarks", (object)request.Remarks ?? DBNull.Value);
                                cmd.Parameters.AddWithValue("@ReleaseDt", (object)request.ReleaseDt ?? DBNull.Value);
                                cmd.Parameters.AddWithValue("@ReleaseBy", (object)request.ReleaseBy ?? DBNull.Value);
                                cmd.Parameters.AddWithValue("@Inactive", (object)request.Inactive ?? DBNull.Value);
                                cmd.Parameters.AddWithValue("@InactiveRemarks", (object)request.InactiveRemarks ?? DBNull.Value);
                                cmd.Parameters.AddWithValue("@InactiveReleaseDt", (object)request.InactiveReleaseDt ?? DBNull.Value);
                                cmd.Parameters.AddWithValue("@InactiveReleaseBy", (object)request.InactiveReleaseBy ?? DBNull.Value);
                                cmd.Parameters.AddWithValue("@PasswordAttempts", (object)request.PasswordAttempts ?? DBNull.Value);
                                cmd.Parameters.AddWithValue("@PasswordUpdatedFlag", (object)request.PasswordUpdatedFlag ?? DBNull.Value);
                                cmd.Parameters.AddWithValue("@UnlockDate", (object)request.UnlockDate ?? DBNull.Value);

                                int rowsAffected = cmd.ExecuteNonQuery();
                              
                            }
                        }
                    }

                    scope.Complete();
                }
                return Content(HttpStatusCode.OK, Common.Common.Messages.USER_UPDATED_SUCCESSFULLY);

            }
            catch (Exception ex)
            {
                Logger.Log($"Error in UpdateUser: {ex.Message}");
                Logger.Log($"StackTrace in UpdateUser: { ex.StackTrace.ToString()}");
                return Content(HttpStatusCode.InternalServerError, Common.Common.Messages.AN_UNEXPECTED_ERROR_OCCURRED);
            }
            //return Ok("Users updated successfully");
        }

        [HttpDelete]
        [Route("api/users/delete")]
        public IHttpActionResult DeleteUser()
        {
            // sync logic here
            return Ok("Users updated successfully");
        }


    }
}   