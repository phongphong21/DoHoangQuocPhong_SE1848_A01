﻿using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public interface IEmployeeRepository
    {
        Employee? GetEmployeeLogin(string Username, string Password);
        List<Employee> GetEmployees();
    }
}
