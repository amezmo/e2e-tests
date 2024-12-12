using System.Diagnostics;
using Amezmo.Tests.Library.Infrastructure.Flows;
using Amezmo.Tests.Library.Infrastructure.Interfaces;
using Amezmo.Tests.Library.Infrastructure.PageModels;
using Microsoft.Extensions.Configuration;

namespace Amezmo.Tests.Library.Infrastructure.TestClasses;

public class LoginFlowTest(IConfiguration config) 
    : PageTest<LoginPageModel>(config)
{
    protected Stopwatch StopWatch = Stopwatch.StartNew();
    private IUserFlow _flow;
    private readonly IConfiguration _config = config;

    [OneTimeSetUp]
    public new async Task OneTimeSetup()
    {
        _flow = new LoginFlow(Page, _config);
        
        StopWatch.Reset();
        StopWatch.Start();

        await _flow.RunAsync();
        StopWatch.Stop();
    }
}