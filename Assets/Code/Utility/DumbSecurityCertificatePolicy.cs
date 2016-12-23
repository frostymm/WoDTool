﻿/*
 * Class for overriding built-in security certificate policy. This is not currently being used but I'm keeping it for reference.
 * */

using UnityEngine;
using System.Collections;
using System;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

public class DumbSecurityCertificatePolicy
{

    public static bool Validator(
        object sender,
    X509Certificate certificate,
    X509Chain chain,
    SslPolicyErrors policyErrors)
    {
        // Just accept and move on...
        return true;
    }

    public static void Instate()
    {
        ServicePointManager.ServerCertificateValidationCallback = Validator;
    }
}