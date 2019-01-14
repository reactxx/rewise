using System;
using System.Collections.Generic;
using System.Linq;

namespace indexer {

  public static class Pseudo {

    static Book[] books;

    // ***** EXPORT
    public static IEnumerable<BookFactsFind> text2Facts(Book book, string query) {
      foreach (var b in books) yield return text2BookFacts(b, query);
    }

    // ***** INDEXES
    static IFact factId2BookFact(Book book, int factId) {
      throw new NotImplementedException();
    }


    static IEnumerable<IFact> groupId2BookFacts(Book book, int groupId) {
      throw new NotImplementedException();
    }

    static IEnumerable<ITrieWord> text2BookWords(Book book, string query) {
      throw new NotImplementedException();
    }

    // returns all childs and self of trie with non epmty factIds
    static IEnumerable<TrieFind> trieChildWords(Object trieContext) {
      throw new NotImplementedException();
    }

    static IEnumerable<IFact> wordId2BookFacts(Book book, int wordUid) {
      throw new NotImplementedException();
    }


    // ***** UTILS

    static BookFactsFind text2BookFacts(Book book, string query) {
      var res = new BookFactsFind { book = book, facts = new Dictionary<int, FactFind>(), query = query };
      var ws = text2BookWords(book, query);
      foreach (var w in ws) {
        if (w.groupIds != null) {
          foreach (var gid in w.groupIds)
            foreach (var fact in groupId2BookFacts(book, gid)) {
              if (!res.facts.TryGetValue(fact.factUid, out FactFind factFind))
                res.facts.Add(fact.factUid, factFind = new FactFind { fact = fact, wordUids = new List<int>() });
              factFind.wordUids.Add(w.wordUid);
            }
        } else {
          foreach (var trieFind in trieChildWords(w.trieContext))
            foreach (var factId in trieFind.factIds) {
              var fact = factId2BookFact(book, factId);
              if (!res.facts.TryGetValue(fact.factUid, out FactFind factFind))
                res.facts.Add(fact.factUid, factFind = new FactFind { fact = fact, prefixes = new List<string>() });
              factFind.prefixes.Add(trieFind.text);
            }
        }
      }
      return res;
    }

  }

  public class BookFactsFind {
    public Book book;
    public string query;
    public Dictionary<int, FactFind> facts;
  }

  public class FactFind {
    public IFact fact;
    public List<int> wordUids; // for stemm search
    public List<string> prefixes; // for trie search
  }

  public interface TrieFind : ITrieWord {
    string text { get; }
  }

  public interface IFact {
    int factUid { get; } // unique fact ID in book
    string text { get; }
    IFactWord[] words { get; }
  }
  public interface IFactWord {
    int wordUid { get; }
    int start { get; } // SOURCE word in range in FactText.text
    int len { get; }
  }

  public interface ITrieWord {
    int wordUid { get; } // unique word ID in book
    int[] factIds { get; } // id of facts, which contains exact word, represented byt this trie node
    int[] groupIds { get; } // stemm groups
    Object trieContext { get; } // context for searching all child IWords with not empty wordUid
  }


}
