using MySql.EntityFrameworkCore.Extensions;
using LibraryRegister.Models;
using Microsoft.EntityFrameworkCore.Design;

namespace LibraryRegister.Services
{
	public class ServiceMapper
	{		
		public class Services : IDesignTimeServices
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
