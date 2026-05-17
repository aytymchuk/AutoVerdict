using System.Reflection;
using NetArchTest.Rules;
using Xunit;
using Shouldly;

namespace AutoVerdikt.Architecture.Tests;

public class ArchitectureTests
{
    private const string ApplicationNamespace = "AutoVerdikt.Application";
    private const string InfrastructureNamespace = "AutoVerdikt.Infrastructure";
    private const string StoreNamespace = "AutoVerdikt.Store";
    private const string WebApiNamespace = "AutoVerdikt.WebApi";

    [Fact]
    public void Application_Should_Not_HaveDependencyOn_OtherProjects()
    {
        var applicationAssembly = Assembly.Load(ApplicationNamespace);

        var result = Types
            .InAssembly(applicationAssembly)
            .ShouldNot()
            .HaveDependencyOnAny(InfrastructureNamespace, StoreNamespace, WebApiNamespace)
            .GetResult();

        result.IsSuccessful.ShouldBeTrue();
    }

    [Fact]
    public void Infrastructure_Should_Not_HaveDependencyOn_Store_Or_WebApi()
    {
        var infrastructureAssembly = Assembly.Load(InfrastructureNamespace);

        var result = Types
            .InAssembly(infrastructureAssembly)
            .ShouldNot()
            .HaveDependencyOnAny(StoreNamespace, WebApiNamespace)
            .GetResult();

        result.IsSuccessful.ShouldBeTrue();
    }

    [Fact]
    public void Store_Should_Not_HaveDependencyOn_Infrastructure_Or_WebApi()
    {
        var storeAssembly = Assembly.Load(StoreNamespace);

        var result = Types
            .InAssembly(storeAssembly)
            .ShouldNot()
            .HaveDependencyOnAny(InfrastructureNamespace, WebApiNamespace)
            .GetResult();

        result.IsSuccessful.ShouldBeTrue();
    }
}
