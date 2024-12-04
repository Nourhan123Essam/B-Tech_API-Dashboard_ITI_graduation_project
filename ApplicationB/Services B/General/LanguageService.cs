using ApplicationB.Contracts_B.General;
using DTOsB.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationB.Services_B.General
{
    public class LanguageService : ILanguageService
    {
        private readonly ILanguageRepository _languageRepository;
        private int _currentLanguageId;

        public LanguageService(ILanguageRepository languageRepository)
        {
            _languageRepository = languageRepository;
            _currentLanguageId = 2; // Default to English
        }

        // Get the currently selected language
        public int GetCurrentLanguageCode()
        {
            return _currentLanguageId;
        }

        // Set language manually (user selection)
        public void SetCurrentLanguageCode(int LanguageId)
        {
            if (LanguageId == null)
            {
                throw new ArgumentException("Invalid language code");
            }

            _currentLanguageId = LanguageId;
        }

        public void SetCurrentLanguageCode(string LanguageCode)
        {
            if (LanguageCode == null)
            {
                throw new ArgumentException("Invalid language code");
            }


            _currentLanguageId = LanguageCode.Contains("ar") ? 1 : 2;


        }




        // Handle default language detection from Accept-Language header
        public async Task<string> SetLanguageFromBrowserAsync(string acceptLanguageHeader)
        {
            string defaultLanguageCode = "en"; // Fallback to a default language ID, e.g., English

            if (!string.IsNullOrEmpty(acceptLanguageHeader))
            {
                // Extract the first language code from the Accept-Language header
                var preferredLanguageCode = acceptLanguageHeader.Split(',').FirstOrDefault()?.Trim();

                if (!string.IsNullOrEmpty(preferredLanguageCode))
                {
                    // Check if the language code exists in the database
                    var language = await _languageRepository.GetByCodeAsync(preferredLanguageCode);

                    // If the language is found, return its ID
                    if (language != null)
                    {
                        return language.Code;
                    }
                }
            }

            // Set the fallback language ID if no matching language was found
            return defaultLanguageCode;
        }


        // Handle user-selected language
        public async Task<int> SetUserSelectedLanguageAsync(int languageId)
        {
            // Check if the selected language exists in the database
            var language = await _languageRepository.GetByIdAsync(languageId);

            if (language != null)
            {
                _currentLanguageId = languageId; // Set the user-selected language
                return languageId;
            }

            return 2; // Fallback to English if the selected language is not found
        }

        public async Task<List<LanguageDto>> GetAllLanguagesAsync()
        {
            var languages = await _languageRepository.GetAllAsync();
            return languages.Select(l => new LanguageDto
            {
                Code = l.Code,
                Id = l.Id,
                Name = l.Name,
            }).ToList();
        }
    } }
