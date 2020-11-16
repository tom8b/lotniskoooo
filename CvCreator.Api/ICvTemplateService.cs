﻿using System;
using System.Threading.Tasks;
using CvCreator.Api.Model;

namespace CvCreator.Api
{
    public interface ICvTemplateService
    {
        Task<CvTemplateModel> AddAsync(Template template, string authorName);
        Task<Template> GetByIdAsync(Guid id);
        Task<bool> FillTemplate(Template template, string username);
        Task<Template> GetFilledTemplate(Guid filledTemplateId);
    }
}