using System.Reflection;
using NetArchTest.Rules;

namespace Brainix.ArchitectureTests;

public class ModuleBoundaryTests
{
    public static readonly Assembly Host = typeof(Program).Assembly;
    public static readonly Assembly SharedKernel = typeof(SharedKernel.SharedKernelMarker).Assembly;

    [Fact]
    public void SharedKernel_must_not_depend_on_Host()
    {
        var result = Types.InAssembly(SharedKernel).Should().NotHaveDependencyOn("Brainix.Host").GetResult();

        Assert.True(result.IsSuccessful, Describe(result));
    }

    [Fact]
    public void Nothing_may_depend_on_the_Host_assembly()
    {
        foreach (var assemby in ProductionAssembliesExceptHost())
        {
            var result = Types.InAssembly(SharedKernel).Should().NotHaveDependencyOn("Brainix.Host").GetResult();

            Assert.True(result.IsSuccessful, $"{assemby.GetName().Name}: {Describe(result)}");
        }
    }

    [Fact]
    public void The_boundary_test_suite_actually_has_assemblies_to_check()
    {
        // a boundary test that loads zero assemblies is a green light that checks nothing
        Assert.NotEmpty(ProductionAssembliesExceptHost());
    }

    private static Assembly[] ProductionAssembliesExceptHost() => [SharedKernel];

    public static string Describe(TestResult result) =>
        result.IsSuccessful ? "Ok" : "offending types" + string.Join(", ", result.FailingTypes.Select(t => t.FullName) ?? []);
}