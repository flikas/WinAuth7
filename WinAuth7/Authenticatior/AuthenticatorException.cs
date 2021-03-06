﻿/*
 * Copyright (C) 2011 Colin Mackie.
 * This software is distributed under the terms of the GNU General Public License.
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsPhoneAuthenticator
{
    /// <summary>
    /// Base Authenticator exception class
    /// </summary>
    public class AuthenticatorException : Exception
    {
        public AuthenticatorException()
            : base()
        {
        }

        public AuthenticatorException(string msg)
            : base(msg)
        {
        }

        public AuthenticatorException(string msg, Exception ex)
            : base(msg, ex)
        {
        }
    }

    /// <summary>
    /// Exception for reading invalid config data
    /// </summary>
    public class InvalidConfigDataException : AuthenticatorException
    {
        public InvalidConfigDataException() : base() { }
    }

    /// <summary>
    /// Exception for invalid user decryption
    /// </summary>
    public class InvalidUserDecryptionException : AuthenticatorException
    {
        public InvalidUserDecryptionException() : base() { }
    }

    /// <summary>
    /// Exception for invalid machine decryption
    /// </summary>
    public class InvalidMachineDecryptionException : AuthenticatorException
    {
        public InvalidMachineDecryptionException() : base() { }
    }

    /// <summary>
    /// Exception for error or unexpected return from server for enroll
    /// </summary>
    public class InvalidEnrollResponseException : AuthenticatorException
    {
        public InvalidEnrollResponseException(string msg) : base(msg) { }
    }

    /// <summary>
    /// Exception for error or unexpected return from server for sync
    /// </summary>
    public class InvalidSyncResponseException : AuthenticatorException
    {
        public InvalidSyncResponseException(string msg) : base(msg) { }
    }

    /// <summary>
    /// Config has been encryoted and we need a key
    /// </summary>
    public class EncrpytedSecretDataException : AuthenticatorException
    {
        public EncrpytedSecretDataException() : base() { }
    }

    /// <summary>
    /// Config decryption bad password
    /// </summary>
    public class BadPasswordException : AuthenticatorException
    {
        public BadPasswordException() : base() { }
    }

    public class InvalidRestoreResponseException : AuthenticatorException
    {
        public InvalidRestoreResponseException(string msg) : base(msg) { }
    }

    public class InvalidRestoreCodeException : InvalidRestoreResponseException
    {
        public InvalidRestoreCodeException(string msg) : base(msg) { }
    }
}
