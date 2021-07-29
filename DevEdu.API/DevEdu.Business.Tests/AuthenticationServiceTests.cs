﻿using DevEdu.Business.Services;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevEdu.Business.Tests
{
    class AuthenticationServiceTests
    {
        private AuthenticationService _auth;
        [SetUp]
        public void Setup()
        {
            _auth = new AuthenticationService();
        }
        [Test]
        public void HashPassword_PasswordAndSalt_ReturnSalt()
        {
            string password = "password";
            byte[] salt = new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };
            var actual = _auth.HashPassword(password, salt);
            string expected = "AAECAwQFBgcICQoLDA0ODyT2cCjwnE2JIl0Ka2bvFeMtEwX+";
            Assert.AreEqual(expected, actual);
        }
        [Test]
        public void HashPassword_WrongSalt_ReturnError()
        {
            string password = "password";
            byte[] salt = new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 };
            Assert.Throws<ArgumentException>(() => _auth.HashPassword(password, salt));
        }

        [Test]
        public void Verify_CorrectPassword_GetTrue()
        {
            var expected = true;
            string hashedPassword = "AAECAwQFBgcICQoLDA0ODyT2cCjwnE2JIl0Ka2bvFeMtEwX+";
            string userPassword = "password";

            var actual = _auth.Verify(hashedPassword, userPassword);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Verify_IncorrectPassword_GetException()
        {
            string hashedPassword = "AAECAwQFBgcICQoLDA0ODyT2cCjwnE2JIl0Ka2bvFeMtEwX";
            string userPassword = "password";

            Assert.Throws<FormatException>(() => _auth.Verify(hashedPassword, userPassword)); ;
        }
    }
}
