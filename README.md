# multi-role-authentication-and-authorization
multi role authentication and authorization with database first boiler plate

Just Run the Database Script and Change the Web.Config and Run the Project


Make Sure One thing Its always be in System.Web

  <system.web>
  
    <authentication mode="Forms">
      <forms loginUrl="Accounts/Login"></forms>
    </authentication>


    <roleManager defaultProvider="usersRoleProvider" enabled="true" >
      <providers>
        <clear/>
        <add name="usersRoleProvider" type="FormAuthentication.Models.UsersRoleProvider"/>
      </providers>
    </roleManager>
    
    <compilation debug="true" targetFramework="4.5.2" />
    <httpRuntime targetFramework="4.5.2" />
  </system.web>
