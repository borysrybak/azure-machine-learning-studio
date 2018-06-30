# .NET DLLs for Azure Machine Learning Studio management

## Overview

### Description

This repository contains a proof of concept project for **[Azure Machine Learning Studio](https://docs.microsoft.com/en-us/azure/machine-learning/studio/what-is-ml-studio)** management.

The solution includes **[.NET Standard 2.0](https://docs.microsoft.com/en-us/dotnet/standard/net-standard)** Class Libraries (*DLLs*),
which can be incorporated in many .NET applications and be used for scale and automate Azure ML Studio workspaces and experiments.

At the moment, the project is in the pre-release phase - :construction: __be aware of possible errors and exceptions before using it!__ :construction:.

### Solution Base Overview

The idea of creating a tool to manage Azure Machine Learning Studio was born during the meeting with our partner **[Soneta/Enova365](https://www.enova.pl/en/)**.

Our partner was looking for solutions that could help automate and scale experiments in Azure ML Studio portal.
After the initial analysis of the available tools, I discovered the only one
that I used later as a base for the current solution
(*source: [PowerShell Module for Azure Machine Learning Studio & Web Services](https://github.com/hning86/azuremlps)*) - this solution was developed for PowerShell users, and the C# code implementation (SDK) helped me as a base to create this project.

## Table of Contents

- [Project](#net-dlls-for-azure-machine-learning-studio-management)  
- [Overview](#overview)  
  - [Description](#description)  
  - [Solution Base Overview](#solution-base-overview)  
- [Table of Contents](#table-of-contents)  
- [Prerequisites](#prerequisites)
- [Usage](#usage)
- [Learnings](#learnings)
- [Credits](#credits)
- [Helpful Materials](#helpful-materials)

## Helpful Materials

- [Framework Design Guidelines](https://docs.microsoft.com/en-us/dotnet/standard/design-guidelines/index)
- [Names of Assemblies and DLLs](https://docs.microsoft.com/en-us/dotnet/standard/design-guidelines/names-of-assemblies-and-dlls)
- [.NET Standard implementation support](https://docs.microsoft.com/en-us/dotnet/standard/net-standard#net-platforms-support)
- [StyleCop analyzes C# source code](https://github.com/StyleCop)
- [Unit testing C# in .NET Core using dotnet test and xUnit](https://docs.microsoft.com/en-us/dotnet/core/testing/unit-testing-with-dotnet-test)
