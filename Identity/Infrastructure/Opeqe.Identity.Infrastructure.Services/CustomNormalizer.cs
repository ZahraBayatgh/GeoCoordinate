﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;

namespace Opeqe.Identity.Infrastructure.Services
{
    public class CustomNormalizer : ILookupNormalizer
    {
        public string NormalizeEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return null;
            }

            email = email.Trim();
            email = fixGmailDots(email);
            email = email.ToUpperInvariant();
            return email;
        }

        public string NormalizeName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return null;
            }

            name = name.Trim();
            //name = name
            //     .RemoveDiacritics()
            //     .CleanUnderLines()
            //     .RemovePunctuation();
            name = name.Trim().Replace(" ", "");
            name = name.ToUpperInvariant();
            return name;
        }

        private static string fixGmailDots(string email)
        {
            email = email.ToLowerInvariant().Trim();
            var emailParts = email.Split('@');
            var name = emailParts[0].Replace(".", string.Empty);

            var plusIndex = name.IndexOf("+", StringComparison.OrdinalIgnoreCase);
            if (plusIndex != -1)
            {
                name = name.Substring(0, plusIndex);
            }

            var emailDomain = emailParts[1];
            emailDomain = emailDomain.Replace("googlemail.com", "gmail.com");

            string[] domainsAllowedDots =
            {
                "gmail.com",
                "facebook.com"
            };

            var isFromDomainsAllowedDots = domainsAllowedDots.Any(domain => emailDomain.Equals(domain));
            return !isFromDomainsAllowedDots ? email : string.Format("{0}@{1}", name, emailDomain);
        }
    }
}