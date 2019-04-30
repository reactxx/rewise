import 'package:test/test.dart' as test;
import "package:github/server.dart" as g;

main() {
  test.test('fake-test', () async {
    //https://github.com/settings/tokens
    var github = g.GitHub(auth: g.Authentication.withToken('aeea8836a6dcf22d8dd9bcbb064ab532bd9bc962'));
    //https://pub.dartlang.org/packages/github
    var repo = await github.repositories.getRepository(g.RepositorySlug('reactxx','reactxx.github.io'));
    test.expect(
      null, test.equals(null));
  });
}
