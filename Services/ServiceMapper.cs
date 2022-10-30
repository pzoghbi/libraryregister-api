using MySql.EntityFrameworkCore.Extensions;
using LibraryRegister.Models;
using Microsoft.EntityFrameworkCore.Design;

namespace LibraryRegister.Services
{
	public class ServiceMapper
	{		
		public class DesignTimeServices : IDesignTimeServices
		{
			public void ConfigureDesignTimeServices(IServiceCollection services)
			{
				services.AddEntityFrameworkMySQL();
				new EntityFrameworkRelationalDesignServicesBuilder(services)
					.TryAddCoreServices();
			}
		}
	}
}
