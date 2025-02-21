global using System.ComponentModel.DataAnnotations;
global using System.IdentityModel.Tokens.Jwt;
global using System.Reflection;
global using System.Security.Claims;
global using System.Text;
global using System.Text.Json;
global using System.Security.Cryptography;
global using Microsoft.Extensions.Diagnostics.HealthChecks;
global using StudyFlow.API.Health;
global using HealthChecks.UI.Client;
global using Microsoft.AspNetCore.Diagnostics.HealthChecks;
global using Hangfire;
global using Hangfire.PostgreSql;
global using HangfireBasicAuthenticationFilter;

global using FluentValidation;
global using FluentValidation.AspNetCore;

global using Google.Apis.Auth;

global using Mapster;
global using MapsterMapper;

global using Microsoft.AspNetCore.Authentication;
global using Microsoft.AspNetCore.Authentication.JwtBearer;
global using Microsoft.AspNetCore.Authorization;
global using Microsoft.AspNetCore.Identity;
global using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.AspNetCore.Mvc.Abstractions;
global using Microsoft.AspNetCore.OpenApi;
global using Microsoft.AspNetCore.WebUtilities;
global using MimeKit;
global using MailKit.Net.Smtp;
global using MailKit.Security;
global using Microsoft.AspNetCore.Identity.UI.Services;

global using Microsoft.EntityFrameworkCore;
global using Microsoft.EntityFrameworkCore.Metadata.Builders;
global using Microsoft.Extensions.Options;

global using Microsoft.IdentityModel.Tokens;
global using Microsoft.OpenApi.Models;

global using Scalar.AspNetCore;
global using Serilog;

global using StudyFlow.API.Abstractions;
global using StudyFlow.API.Abstractions.Const;
global using StudyFlow.API.Contracts.Authentications;
global using StudyFlow.API.Contracts.Users;
global using StudyFlow.API.Contracts.Students;

global using StudyFlow.API.Entities;
global using StudyFlow.API.Errors;
global using StudyFlow.API.Extentions;
global using StudyFlow.API.Middlewares;
global using StudyFlow.API.OpenApiTransformer;
global using StudyFlow.API.Persistences.Context;
global using StudyFlow.API.Repository.Implementations;
global using StudyFlow.API.Repository.Interfaces;
global using StudyFlow.API.Contracts.Roles;
global using StudyFlow.API.Helpers;