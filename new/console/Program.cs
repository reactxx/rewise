namespace console {
  class Program {
    static void Main() {
      using (var imp = new Impersonator.Impersonator("pavel", "LANGMaster", "zvahov88_")) {
        WordNetIndex.run();
      }
    }
  }
}
