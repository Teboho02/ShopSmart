// Tests share a data directory on disk, so parallel execution causes interference.
// Run all test collections sequentially.
[assembly: Xunit.CollectionBehavior(DisableTestParallelization = true)]
