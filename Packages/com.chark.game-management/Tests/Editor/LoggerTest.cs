using System;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace CHARK.GameManagement.Tests.Editor
{
    internal sealed class LoggerTest
    {
        [SetUp]
        public void SetUp()
        {
            LogAssert.ignoreFailingMessages = true;
        }

        [Test]
        public void ShouldPrintInfoMessageUsingThis()
        {
            GameManager.LogWith(this).LogInfo("Info"); // line 19
            LogAssert.Expect(LogType.Log, "[LoggerTest:19]: Info");
        }

        [Test]
        public void ShouldPrintWarnMessageUsingThis()
        {
            GameManager.LogWith(this).LogWarning("Warn"); // line 26
            LogAssert.Expect(LogType.Warning, "[LoggerTest:26]: Warn");
        }

        [Test]
        public void ShouldPrintErrorMessageUsingThis()
        {
            GameManager.LogWith(this).LogError("Error"); // line 33
            LogAssert.Expect(LogType.Error, "[LoggerTest:33]: Error");
        }

        [Test]
        public void ShouldPrintExceptionMessageUsingThis()
        {
            GameManager.LogWith(this).LogError(new Exception("hi")); // line 40
            LogAssert.Expect(LogType.Exception, "Exception: hi");
        }

        [Test]
        public void ShouldPrintInfoMessageUsingType()
        {
            GameManager.LogWith(GetType()).LogInfo("Info"); // line 47
            LogAssert.Expect(LogType.Log, "[LoggerTest:47]: Info");
        }

        [Test]
        public void ShouldPrintWarnMessageUsingType()
        {
            GameManager.LogWith(GetType()).LogWarning("Warn"); // line 54
            LogAssert.Expect(LogType.Warning, "[LoggerTest:54]: Warn");
        }

        [Test]
        public void ShouldPrintErrorMessageUsingType()
        {
            GameManager.LogWith(GetType()).LogError("Error"); // line 61
            LogAssert.Expect(LogType.Error, "[LoggerTest:61]: Error");
        }

        [Test]
        public void ShouldPrintExceptionMessageUsingType()
        {
            GameManager.LogWith(GetType()).LogError(new Exception("hi")); // line 68
            LogAssert.Expect(LogType.Exception, "Exception: hi");
        }
    }
}
