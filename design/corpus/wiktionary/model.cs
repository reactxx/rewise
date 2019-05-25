// inheritance: https://weblogs.asp.net/manavi/inheritance-mapping-strategies-with-entity-framework-code-first-ctp5-part-1-table-per-hierarchy-tph

namespace WiktModel {

  public abstract partial class Helper {
    public int Id { get; set; }
  }

  // Page
  public partial class Page: Helper {
    public string Title { get; set; }
  }

  // Entry
  public partial class Entry : Helper {

    public int PageId { get; set; }
    public byte NymType { get; set; }

    // ignore dbnary:partOfSpeech
    // values UriValues[lexinfo:noun, ...]
    // http://kaiko.getalp.org/sparql?default-graph-uri=&query=PREFIX+ontolex%3A+%3Chttp%3A%2F%2Fwww.w3.org%2Fns%2Flemon%2Fontolex%23%3E%0D%0APREFIX+dbnary%3A+%3Chttp%3A%2F%2Fkaiko.getalp.org%2Fdbnary%23%3E%0D%0APREFIX+lexinfo%3A+%3Chttp%3A%2F%2Fwww.lexinfo.net%2Fontology%2F2.0%2Flexinfo%23%3E%0D%0APREFIX+rdf%3A+%3Chttp%3A%2F%2Fwww.w3.org%2F1999%2F02%2F22-rdf-syntax-ns%23%3E%0D%0APREFIX+olia%3A+%3Chttp%3A%2F%2Fpurl.org%2Folia%2Folia.owl%23%3E%0D%0APREFIX+skos%3A+%3Chttp%3A%2F%2Fwww.w3.org%2F2004%2F02%2Fskos%2Fcore%23%3E%0D%0APREFIX+terms%3A+++%3Chttp%3A%2F%2Fpurl.org%2Fdc%2Fterms%2F%3E%0D%0APREFIX+lime%3A+%3Chttp%3A%2F%2Fwww.w3.org%2Fns%2Flemon%2Flime%23%3E%0D%0APREFIX+vartrans%3A+++%3Chttp%3A%2F%2Fwww.w3.org%2Fns%2Flemon%2Fvartrans%23%3E%0D%0APREFIX+prot%3A+++%3Chttp%3A%2F%2Fproton.semanticweb.org%2Fprotonsys%23%3E%0D%0APREFIX+xsd%3A+%3Chttp%3A%2F%2Fwww.w3.org%2F2001%2FXMLSchema%23%3E%0D%0APREFIX+iso%3A+%3Chttp%3A%2F%2Flexvo.org%2Fid%2Fiso639-3%2F%3E%0D%0A%0D%0ASELECT+%3Fo2+%28count%28%3Fo2%29+as+%3Foo2%29%0D%0AWHERE+%7B%0D%0A++%3Fs+lexinfo%3ApartOfSpeech+%3Fo2+.%0D%0A++%23+FILTER+NOT+EXISTS+%7B+%3Fo+a+rdf%3AProperty+%7D%0D%0A++%23+bind%28coalesce%28%3Fo1%2C%3F%29+as+%3Foo1%29%0D%0A%7D%0D%0AGROUP+BY+%3Fo2&format=text%2Fhtml&timeout=0&debug=on
    // what is it??? lexinfo:partOfSpeech=ontolex:LexicalEntry, see http://kaiko.getalp.org/sparql?default-graph-uri=&query=PREFIX+ontolex%3A+%3Chttp%3A%2F%2Fwww.w3.org%2Fns%2Flemon%2Fontolex%23%3E%0D%0APREFIX+dbnary%3A+%3Chttp%3A%2F%2Fkaiko.getalp.org%2Fdbnary%23%3E%0D%0APREFIX+lexinfo%3A+%3Chttp%3A%2F%2Fwww.lexinfo.net%2Fontology%2F2.0%2Flexinfo%23%3E%0D%0APREFIX+rdf%3A+%3Chttp%3A%2F%2Fwww.w3.org%2F1999%2F02%2F22-rdf-syntax-ns%23%3E%0D%0APREFIX+olia%3A+%3Chttp%3A%2F%2Fpurl.org%2Folia%2Folia.owl%23%3E%0D%0APREFIX+skos%3A+%3Chttp%3A%2F%2Fwww.w3.org%2F2004%2F02%2Fskos%2Fcore%23%3E%0D%0APREFIX+terms%3A+++%3Chttp%3A%2F%2Fpurl.org%2Fdc%2Fterms%2F%3E%0D%0APREFIX+lime%3A+%3Chttp%3A%2F%2Fwww.w3.org%2Fns%2Flemon%2Flime%23%3E%0D%0APREFIX+vartrans%3A+++%3Chttp%3A%2F%2Fwww.w3.org%2Fns%2Flemon%2Fvartrans%23%3E%0D%0APREFIX+prot%3A+++%3Chttp%3A%2F%2Fproton.semanticweb.org%2Fprotonsys%23%3E%0D%0APREFIX+xsd%3A+%3Chttp%3A%2F%2Fwww.w3.org%2F2001%2FXMLSchema%23%3E%0D%0APREFIX+iso%3A+%3Chttp%3A%2F%2Flexvo.org%2Fid%2Fiso639-3%2F%3E%0D%0A%0D%0ASELECT+%28count%28%3Fs%29+as+%3Fcs%29%0D%0AWHERE+%7B%0D%0A++%3Fs+lexinfo%3ApartOfSpeech+ontolex%3ALexicalEntry+.%0D%0A++%23+FILTER+NOT+EXISTS+%7B+%3Fo+a+rdf%3AProperty+%7D%0D%0A++%23+bind%28coalesce%28%3Fo1%2C%3F%29+as+%3Foo1%29%0D%0A%7D%0D%0A%23GROUP+BY+%3Fos&format=text%2Fhtml&timeout=0&debug=on
    public byte PartOfSpeech { get; set; } // lexinfo:partOfSpeech

    // for BG only: text slova. V canonicalForm.writtenRep is LATIN transcription
    // http://kaiko.getalp.org/sparql?default-graph-uri=&query=PREFIX+ontolex%3A+%3Chttp%3A%2F%2Fwww.w3.org%2Fns%2Flemon%2Fontolex%23%3E%0D%0APREFIX+dbnary%3A+%3Chttp%3A%2F%2Fkaiko.getalp.org%2Fdbnary%23%3E%0D%0APREFIX+lexinfo%3A+%3Chttp%3A%2F%2Fwww.lexinfo.net%2Fontology%2F2.0%2Flexinfo%23%3E%0D%0APREFIX+rdf%3A+%3Chttp%3A%2F%2Fwww.w3.org%2F1999%2F02%2F22-rdf-syntax-ns%23%3E%0D%0APREFIX+olia%3A+%3Chttp%3A%2F%2Fpurl.org%2Folia%2Folia.owl%23%3E%0D%0APREFIX+skos%3A+%3Chttp%3A%2F%2Fwww.w3.org%2F2004%2F02%2Fskos%2Fcore%23%3E%0D%0APREFIX+terms%3A+++%3Chttp%3A%2F%2Fpurl.org%2Fdc%2Fterms%2F%3E%0D%0APREFIX+lime%3A+%3Chttp%3A%2F%2Fwww.w3.org%2Fns%2Flemon%2Flime%23%3E%0D%0APREFIX+vartrans%3A+++%3Chttp%3A%2F%2Fwww.w3.org%2Fns%2Flemon%2Fvartrans%23%3E%0D%0APREFIX+prot%3A+++%3Chttp%3A%2F%2Fproton.semanticweb.org%2Fprotonsys%23%3E%0D%0APREFIX+xsd%3A+%3Chttp%3A%2F%2Fwww.w3.org%2F2001%2FXMLSchema%23%3E%0D%0APREFIX+iso%3A+%3Chttp%3A%2F%2Flexvo.org%2Fid%2Fiso639-3%2F%3E%0D%0A%0D%0ASELECT+%3Fo1+%3Fv%0D%0AWHERE+%7B%0D%0A++%3Fs+ontolex%3AwrittenRep+%3Fo1+.%0D%0A++%3Fs+a+ontolex%3ALexicalEntry.+%0D%0A%3Fs+ontolex%3AcanonicalForm+%3Fcf+.%0D%0A%3Fcf+ontolex%3AwrittenRep+%3Fv+.%0D%0A++%23+FILTER+NOT+EXISTS+%7B+%3Fo+a+rdf%3AProperty+%7D%0D%0A++%23+bind%28coalesce%28%3Fo1%2C%22%3F%22%29+as+%3Foo1%29%0D%0A%7D%0D%0A%23+limit+100%0D%0A&format=text%2Fhtml&timeout=0&debug=on
    public string WrittenRep { get; set; } // ontolex:writtenRep - rdf:langString

    // for lexinfo:noun and properNoun: UriValues[lexinfo:feminine,masculine,neuter]
    // http://kaiko.getalp.org/sparql?default-graph-uri=&query=PREFIX+ontolex%3A+%3Chttp%3A%2F%2Fwww.w3.org%2Fns%2Flemon%2Fontolex%23%3E%0D%0APREFIX+dbnary%3A+%3Chttp%3A%2F%2Fkaiko.getalp.org%2Fdbnary%23%3E%0D%0APREFIX+lexinfo%3A+%3Chttp%3A%2F%2Fwww.lexinfo.net%2Fontology%2F2.0%2Flexinfo%23%3E%0D%0APREFIX+rdf%3A+%3Chttp%3A%2F%2Fwww.w3.org%2F1999%2F02%2F22-rdf-syntax-ns%23%3E%0D%0APREFIX+olia%3A+%3Chttp%3A%2F%2Fpurl.org%2Folia%2Folia.owl%23%3E%0D%0APREFIX+skos%3A+%3Chttp%3A%2F%2Fwww.w3.org%2F2004%2F02%2Fskos%2Fcore%23%3E%0D%0APREFIX+terms%3A+++%3Chttp%3A%2F%2Fpurl.org%2Fdc%2Fterms%2F%3E%0D%0APREFIX+lime%3A+%3Chttp%3A%2F%2Fwww.w3.org%2Fns%2Flemon%2Flime%23%3E%0D%0APREFIX+vartrans%3A+++%3Chttp%3A%2F%2Fwww.w3.org%2Fns%2Flemon%2Fvartrans%23%3E%0D%0APREFIX+prot%3A+++%3Chttp%3A%2F%2Fproton.semanticweb.org%2Fprotonsys%23%3E%0D%0APREFIX+xsd%3A+%3Chttp%3A%2F%2Fwww.w3.org%2F2001%2FXMLSchema%23%3E%0D%0APREFIX+iso%3A+%3Chttp%3A%2F%2Flexvo.org%2Fid%2Fiso639-3%2F%3E%0D%0A%0D%0ASELECT+%3Fo2+%28count%28%3Fo2%29+as+%3Foo2%29%0D%0AWHERE+%7B%0D%0A++%3Fs+lexinfo%3Agender+%3Fo1+.%0D%0A++%3Fs+lexinfo%3ApartOfSpeech+%3Fo2+.%0D%0A++%23+FILTER+NOT+EXISTS+%7B+%3Fo+a+rdf%3AProperty+%7D%0D%0A++%23+bind%28coalesce%28%3Fo1%2C%22%3F%22%29+as+%3Foo1%29%0D%0A%7D%0D%0AGROUP+BY+%3Fo2%0D%0A&format=text%2Fhtml&timeout=0&debug=on
    public byte Gender { get; set; } // lexinfo:gender

    // for lexinfo:noun and properNoun: UriValues[olia:Countable x Uncountable]
    // http://kaiko.getalp.org/sparql?default-graph-uri=&query=PREFIX+ontolex%3A+%3Chttp%3A%2F%2Fwww.w3.org%2Fns%2Flemon%2Fontolex%23%3E%0D%0APREFIX+dbnary%3A+%3Chttp%3A%2F%2Fkaiko.getalp.org%2Fdbnary%23%3E%0D%0APREFIX+lexinfo%3A+%3Chttp%3A%2F%2Fwww.lexinfo.net%2Fontology%2F2.0%2Flexinfo%23%3E%0D%0APREFIX+rdf%3A+%3Chttp%3A%2F%2Fwww.w3.org%2F1999%2F02%2F22-rdf-syntax-ns%23%3E%0D%0APREFIX+olia%3A+%3Chttp%3A%2F%2Fpurl.org%2Folia%2Folia.owl%23%3E%0D%0APREFIX+skos%3A+%3Chttp%3A%2F%2Fwww.w3.org%2F2004%2F02%2Fskos%2Fcore%23%3E%0D%0APREFIX+terms%3A+++%3Chttp%3A%2F%2Fpurl.org%2Fdc%2Fterms%2F%3E%0D%0APREFIX+lime%3A+%3Chttp%3A%2F%2Fwww.w3.org%2Fns%2Flemon%2Flime%23%3E%0D%0APREFIX+vartrans%3A+++%3Chttp%3A%2F%2Fwww.w3.org%2Fns%2Flemon%2Fvartrans%23%3E%0D%0APREFIX+prot%3A+++%3Chttp%3A%2F%2Fproton.semanticweb.org%2Fprotonsys%23%3E%0D%0APREFIX+xsd%3A+%3Chttp%3A%2F%2Fwww.w3.org%2F2001%2FXMLSchema%23%3E%0D%0APREFIX+iso%3A+%3Chttp%3A%2F%2Flexvo.org%2Fid%2Fiso639-3%2F%3E%0D%0A%0D%0ASELECT+%3Fo2+%28count%28%3Fo2%29+as+%3Foo2%29%0D%0AWHERE+%7B%0D%0A++%3Fs+olia%3AhasCountability+%3Fo1+.%0D%0A++%3Fs+lexinfo%3ApartOfSpeech+%3Fo2+.%0D%0A++%23+FILTER+NOT+EXISTS+%7B+%3Fo+a+rdf%3AProperty+%7D%0D%0A++%23+bind%28coalesce%28%3Fo1%2C%22%3F%22%29+as+%3Foo1%29%0D%0A%7D%0D%0AGROUP+BY+%3Fo2%0D%0A&format=text%2Fhtml&timeout=0&debug=on
    public byte HasCountability { get; set; } // olia:hasCountability

    // UriValues[olia:Uninflected, olia:???Inflection...]
    // ignore, 180x only
    // http://kaiko.getalp.org/sparql?default-graph-uri=&query=PREFIX+ontolex%3A+%3Chttp%3A%2F%2Fwww.w3.org%2Fns%2Flemon%2Fontolex%23%3E%0D%0APREFIX+dbnary%3A+%3Chttp%3A%2F%2Fkaiko.getalp.org%2Fdbnary%23%3E%0D%0APREFIX+lexinfo%3A+%3Chttp%3A%2F%2Fwww.lexinfo.net%2Fontology%2F2.0%2Flexinfo%23%3E%0D%0APREFIX+rdf%3A+%3Chttp%3A%2F%2Fwww.w3.org%2F1999%2F02%2F22-rdf-syntax-ns%23%3E%0D%0APREFIX+olia%3A+%3Chttp%3A%2F%2Fpurl.org%2Folia%2Folia.owl%23%3E%0D%0APREFIX+skos%3A+%3Chttp%3A%2F%2Fwww.w3.org%2F2004%2F02%2Fskos%2Fcore%23%3E%0D%0APREFIX+terms%3A+++%3Chttp%3A%2F%2Fpurl.org%2Fdc%2Fterms%2F%3E%0D%0APREFIX+lime%3A+%3Chttp%3A%2F%2Fwww.w3.org%2Fns%2Flemon%2Flime%23%3E%0D%0APREFIX+vartrans%3A+++%3Chttp%3A%2F%2Fwww.w3.org%2Fns%2Flemon%2Fvartrans%23%3E%0D%0APREFIX+prot%3A+++%3Chttp%3A%2F%2Fproton.semanticweb.org%2Fprotonsys%23%3E%0D%0APREFIX+xsd%3A+%3Chttp%3A%2F%2Fwww.w3.org%2F2001%2FXMLSchema%23%3E%0D%0APREFIX+iso%3A+%3Chttp%3A%2F%2Flexvo.org%2Fid%2Fiso639-3%2F%3E%0D%0A%0D%0ASELECT+%3Fo%0D%0AWHERE+%7B%0D%0A++%3Fs+olia%3AhasInflectionType+%3Fo+.%0D%0A%23+%3Fo+%3Fp+%3Fo2+.%0D%0A%23+++%3Fs+a+%3Ft+.%0D%0A++%23+FILTER+NOT+EXISTS+%7B+%3Fo+a+rdf%3AProperty+%7D%0D%0A++%23+bind%28coalesce%28%3Fo1%2C%3F%29+as+%3Foo1%29%0D%0A%7D%0D%0A%23+GROUP+BY+%3Fo2%0D%0Alimit+1000&format=text%2Fhtml&timeout=0&debug=on
    //public byte HasInflectionType { get; set; } // olia:hasInflectionType

    // for lexinfo:verb and lexinfo:participle: UriValues[olia:Separable,olia:NonSeparable]
    // ignore, 273x only
    //public byte HasSeparability { get; set; } // olia:hasSeparability - @olia,

    // ignore, 64x only
    //public byte HasValency { get; set; } // olia:hasValency - @olia,
    // ignore, 10x only
    //public byte HasVoice { get; set; } // olia:hasVoice - @olia,

    // for what is it?
    // http://kaiko.getalp.org/sparql?default-graph-uri=&query=PREFIX+ontolex%3A+%3Chttp%3A%2F%2Fwww.w3.org%2Fns%2Flemon%2Fontolex%23%3E%0D%0APREFIX+dbnary%3A+%3Chttp%3A%2F%2Fkaiko.getalp.org%2Fdbnary%23%3E%0D%0APREFIX+lexinfo%3A+%3Chttp%3A%2F%2Fwww.lexinfo.net%2Fontology%2F2.0%2Flexinfo%23%3E%0D%0APREFIX+rdf%3A+%3Chttp%3A%2F%2Fwww.w3.org%2F1999%2F02%2F22-rdf-syntax-ns%23%3E%0D%0APREFIX+olia%3A+%3Chttp%3A%2F%2Fpurl.org%2Folia%2Folia.owl%23%3E%0D%0APREFIX+skos%3A+%3Chttp%3A%2F%2Fwww.w3.org%2F2004%2F02%2Fskos%2Fcore%23%3E%0D%0APREFIX+terms%3A+++%3Chttp%3A%2F%2Fpurl.org%2Fdc%2Fterms%2F%3E%0D%0APREFIX+lime%3A+%3Chttp%3A%2F%2Fwww.w3.org%2Fns%2Flemon%2Flime%23%3E%0D%0APREFIX+vartrans%3A+++%3Chttp%3A%2F%2Fwww.w3.org%2Fns%2Flemon%2Fvartrans%23%3E%0D%0APREFIX+prot%3A+++%3Chttp%3A%2F%2Fproton.semanticweb.org%2Fprotonsys%23%3E%0D%0APREFIX+xsd%3A+%3Chttp%3A%2F%2Fwww.w3.org%2F2001%2FXMLSchema%23%3E%0D%0APREFIX+iso%3A+%3Chttp%3A%2F%2Flexvo.org%2Fid%2Fiso639-3%2F%3E%0D%0A%0D%0ASELECT+%3Fo2+%0D%0AWHERE+%7B%0D%0A++%3Fs+vartrans%3AlexicalRel+%3Fo+.%0D%0A%3Fo+%3Fp+%3Fo2+.%0D%0A%23+++%3Fs+a+%3Ft+.%0D%0A++%23+FILTER+NOT+EXISTS+%7B+%3Fo+a+rdf%3AProperty+%7D%0D%0A++%23+bind%28coalesce%28%3Fo1%2C%3F%29+as+%3Foo1%29%0D%0A%7D%0D%0A%23+GROUP+BY+%3Fo2%0D%0Alimit+1000&format=text%2Fhtml&timeout=0&debug=on
    //public string LexicalRel { get; set; } // vartrans:lexicalRel
  }

  // Translation
  public partial class Translation : Helper {
    public int OfEntry { get; set; }
    public int OfPage { get; set; }
    public int OfSense { get; set; }

    public string WrittenForm { get; set; } // dbnary:writtenForm - rdf:langString, dbnary:writtenForm - xsd:string
    public string Usage { get; set; } // dbnary:usage - xsd:string
    public string TargetLanguage { get; set; } // dbnary:targetLanguage OR dbnary:targetLanguageCode
  }

  // Sense
  public partial class Sense : Helper {
    public int SenseNumber { get; set; } // dbnary:senseNumber - xsd:string
    public string Definition { get; set; } // skos:definition - "blank"
    public string Example { get; set; } // skos:example - "blank"
  }

  public partial class Statement : Helper {
    public int SubjectId { get; set; } // Page or Entry id
    public int PageObjectId { get; set; }
    public byte NymType { get; set; }
    public string Usage { get; set; } // SV only, 180 cases only
  }

  //************** M:N
  // 11 093 069 kaiko.getalp.org/sparql?default-graph-uri=&query=PREFIX+ontolex%3A+<http%3A%2F%2Fwww.w3.org%2Fns%2Flemon%2Fontolex%23>%0D%0APREFIX+dbnary%3A+<http%3A%2F%2Fkaiko.getalp.org%2Fdbnary%23>%0D%0APREFIX+lexinfo%3A+<http%3A%2F%2Fwww.lexinfo.net%2Fontology%2F2.0%2Flexinfo%23>%0D%0APREFIX+rdf%3A+<http%3A%2F%2Fwww.w3.org%2F1999%2F02%2F22-rdf-syntax-ns%23>%0D%0APREFIX+olia%3A+<http%3A%2F%2Fpurl.org%2Folia%2Folia.owl%23>%0D%0APREFIX+skos%3A+<http%3A%2F%2Fwww.w3.org%2F2004%2F02%2Fskos%2Fcore%23>%0D%0APREFIX+terms%3A+++<http%3A%2F%2Fpurl.org%2Fdc%2Fterms%2F>%0D%0APREFIX+lime%3A+<http%3A%2F%2Fwww.w3.org%2Fns%2Flemon%2Flime%23>%0D%0APREFIX+vartrans%3A+++<http%3A%2F%2Fwww.w3.org%2Fns%2Flemon%2Fvartrans%23>%0D%0APREFIX+prot%3A+++<http%3A%2F%2Fproton.semanticweb.org%2Fprotonsys%23>%0D%0APREFIX+xsd%3A+<http%3A%2F%2Fwww.w3.org%2F2001%2FXMLSchema%23>%0D%0APREFIX+iso%3A+<http%3A%2F%2Flexvo.org%2Fid%2Fiso639-3%2F>%0D%0A%0D%0ASELECT+%0D%0A%28count%28%3Fp%29+as+%3Fcp%29%0D%0A%23distinct+%3Fp%0D%0AWHERE+%7B%0D%0A++%3Fs+a+%3Ft+.%0D%0A++%3Fs+%3Fp+%3Fo+.%0D%0A++%3Fo+a+ontolex%3ALexicalSense+.%0D%0A++%23+FILTER+NOT+EXISTS+%7B+%3Fo+a+rdf%3AProperty+%7D%0D%0A++%23+bind%28coalesce%28%3Fo1%2C%3F%29+as+%3Foo1%29%0D%0Afilter%28%3Fp%21%3Ddbnary%3Adescribes%29%0D%0Afilter%28%3Ft%21%3Ddbnary%3APage%29%0D%0Afilter%28%3Ft%21%3Dontolex%3ALexicalSense%29%0D%0A%7D&format=text%2Fhtml&timeout=0&debug=on
  public class Entry_Sense {
    public int EntryId { get; set; }
    public int SenseId { get; set; }

    public byte EntryType { get; set; }
  }

  // 109914
  // Sense to Sense: 0 http://kaiko.getalp.org/sparql?default-graph-uri=&query=PREFIX+ontolex%3A+%3Chttp%3A%2F%2Fwww.w3.org%2Fns%2Flemon%2Fontolex%23%3E%0D%0APREFIX+dbnary%3A+%3Chttp%3A%2F%2Fkaiko.getalp.org%2Fdbnary%23%3E%0D%0APREFIX+lexinfo%3A+%3Chttp%3A%2F%2Fwww.lexinfo.net%2Fontology%2F2.0%2Flexinfo%23%3E%0D%0APREFIX+rdf%3A+%3Chttp%3A%2F%2Fwww.w3.org%2F1999%2F02%2F22-rdf-syntax-ns%23%3E%0D%0APREFIX+olia%3A+%3Chttp%3A%2F%2Fpurl.org%2Folia%2Folia.owl%23%3E%0D%0APREFIX+skos%3A+%3Chttp%3A%2F%2Fwww.w3.org%2F2004%2F02%2Fskos%2Fcore%23%3E%0D%0APREFIX+terms%3A+++%3Chttp%3A%2F%2Fpurl.org%2Fdc%2Fterms%2F%3E%0D%0APREFIX+lime%3A+%3Chttp%3A%2F%2Fwww.w3.org%2Fns%2Flemon%2Flime%23%3E%0D%0APREFIX+vartrans%3A+++%3Chttp%3A%2F%2Fwww.w3.org%2Fns%2Flemon%2Fvartrans%23%3E%0D%0APREFIX+prot%3A+++%3Chttp%3A%2F%2Fproton.semanticweb.org%2Fprotonsys%23%3E%0D%0APREFIX+xsd%3A+%3Chttp%3A%2F%2Fwww.w3.org%2F2001%2FXMLSchema%23%3E%0D%0APREFIX+iso%3A+%3Chttp%3A%2F%2Flexvo.org%2Fid%2Fiso639-3%2F%3E%0D%0A%0D%0ASELECT+%0D%0A%28count%28%3Fp%29+as+%3Fcp%29%0D%0A%23distinct+%3Fp%0D%0AWHERE+%7B%0D%0A++%3Fs+a+%3Ft+.%0D%0A++%3Fs+%3Fp+%3Fo+.%0D%0A++%3Fo+a+ontolex%3ALexicalSense+.%0D%0A++%23+FILTER+NOT+EXISTS+%7B+%3Fo+a+rdf%3AProperty+%7D%0D%0A++%23+bind%28coalesce%28%3Fo1%2C%3F%29+as+%3Foo1%29%0D%0Afilter%28%3Fp%21%3Ddbnary%3Adescribes%29%0D%0A%23+filter%28%3Ft%21%3Ddbnary%3APage%29%0D%0Afilter%28%3Ft%3Dontolex%3ALexicalSense%29%0D%0A%7D&format=text%2Fhtml&timeout=0&debug=on
  // Sense to Entry: 0 kaiko.getalp.org/sparql?default-graph-uri=&query=PREFIX+ontolex%3A+<http%3A%2F%2Fwww.w3.org%2Fns%2Flemon%2Fontolex%23>%0D%0APREFIX+dbnary%3A+<http%3A%2F%2Fkaiko.getalp.org%2Fdbnary%23>%0D%0APREFIX+lexinfo%3A+<http%3A%2F%2Fwww.lexinfo.net%2Fontology%2F2.0%2Flexinfo%23>%0D%0APREFIX+rdf%3A+<http%3A%2F%2Fwww.w3.org%2F1999%2F02%2F22-rdf-syntax-ns%23>%0D%0APREFIX+olia%3A+<http%3A%2F%2Fpurl.org%2Folia%2Folia.owl%23>%0D%0APREFIX+skos%3A+<http%3A%2F%2Fwww.w3.org%2F2004%2F02%2Fskos%2Fcore%23>%0D%0APREFIX+terms%3A+++<http%3A%2F%2Fpurl.org%2Fdc%2Fterms%2F>%0D%0APREFIX+lime%3A+<http%3A%2F%2Fwww.w3.org%2Fns%2Flemon%2Flime%23>%0D%0APREFIX+vartrans%3A+++<http%3A%2F%2Fwww.w3.org%2Fns%2Flemon%2Fvartrans%23>%0D%0APREFIX+prot%3A+++<http%3A%2F%2Fproton.semanticweb.org%2Fprotonsys%23>%0D%0APREFIX+xsd%3A+<http%3A%2F%2Fwww.w3.org%2F2001%2FXMLSchema%23>%0D%0APREFIX+iso%3A+<http%3A%2F%2Flexvo.org%2Fid%2Fiso639-3%2F>%0D%0A%0D%0ASELECT+%0D%0A%28count%28%3Fp%29+as+%3Fcp%29%0D%0A%23distinct+%3Fp%0D%0AWHERE+%7B%0D%0A++%3Fs+a+ontolex%3ALexicalSense+.%0D%0A++%3Fs+%3Fp+%3Fo+.%0D%0A++%3Fo+a+%3Ft+.%0D%0A++%23+FILTER+NOT+EXISTS+%7B+%3Fo+a+rdf%3AProperty+%7D%0D%0A++%23+bind%28coalesce%28%3Fo1%2C%3F%29+as+%3Foo1%29%0D%0Afilter%28%3Fp%21%3Ddbnary%3Adescribes%29%0D%0Afilter%28%3Ft%21%3Ddbnary%3APage%29%0D%0Afilter%28%3Ft%21%3Dontolex%3ALexicalSense%29%0D%0A%7D&format=text%2Fhtml&timeout=0&debug=on 
  public class Entry_Page {
    public int EntryId { get; set; }
    public int ToPageId { get; set; }

    public byte EntryType { get; set; }
    public byte NymType { get; set; }
  }

  // 1 090 893 http://kaiko.getalp.org/sparql?default-graph-uri=&query=PREFIX+ontolex%3A+%3Chttp%3A%2F%2Fwww.w3.org%2Fns%2Flemon%2Fontolex%23%3E%0D%0APREFIX+dbnary%3A+%3Chttp%3A%2F%2Fkaiko.getalp.org%2Fdbnary%23%3E%0D%0APREFIX+lexinfo%3A+%3Chttp%3A%2F%2Fwww.lexinfo.net%2Fontology%2F2.0%2Flexinfo%23%3E%0D%0APREFIX+rdf%3A+%3Chttp%3A%2F%2Fwww.w3.org%2F1999%2F02%2F22-rdf-syntax-ns%23%3E%0D%0APREFIX+olia%3A+%3Chttp%3A%2F%2Fpurl.org%2Folia%2Folia.owl%23%3E%0D%0APREFIX+skos%3A+%3Chttp%3A%2F%2Fwww.w3.org%2F2004%2F02%2Fskos%2Fcore%23%3E%0D%0APREFIX+terms%3A+++%3Chttp%3A%2F%2Fpurl.org%2Fdc%2Fterms%2F%3E%0D%0APREFIX+lime%3A+%3Chttp%3A%2F%2Fwww.w3.org%2Fns%2Flemon%2Flime%23%3E%0D%0APREFIX+vartrans%3A+++%3Chttp%3A%2F%2Fwww.w3.org%2Fns%2Flemon%2Fvartrans%23%3E%0D%0APREFIX+prot%3A+++%3Chttp%3A%2F%2Fproton.semanticweb.org%2Fprotonsys%23%3E%0D%0APREFIX+xsd%3A+%3Chttp%3A%2F%2Fwww.w3.org%2F2001%2FXMLSchema%23%3E%0D%0APREFIX+iso%3A+%3Chttp%3A%2F%2Flexvo.org%2Fid%2Fiso639-3%2F%3E%0D%0A%0D%0ASELECT+%0D%0A%28count%28%3Fp%29+as+%3Fcp%29%0D%0A%23distinct+%3Fp%0D%0AWHERE+%7B%0D%0A++%3Fs+a+%3Ft+.%0D%0A++%3Fs+%3Fp+%3Fo+.%0D%0A++%3Fo+a+dbnary%3APage+.%0D%0A++%23+FILTER+NOT+EXISTS+%7B+%3Fo+a+rdf%3AProperty+%7D%0D%0A++%23+bind%28coalesce%28%3Fo1%2C%3F%29+as+%3Foo1%29%0D%0Afilter%28%3Fp%21%3Ddbnary%3Adescribes%29%0D%0Afilter%28%3Ft%21%3Ddbnary%3APage%29%0D%0Afilter%28%3Ft%3Dontolex%3ALexicalSense%29%0D%0A%7D&format=text%2Fhtml&timeout=0&debug=on
  public class Sense_Page {
    public int SenseId { get; set; }
    public int ToPageId { get; set; }

    public byte NymType { get; set; }
  }

  // Page to Page: EN only, 45415 http://kaiko.getalp.org/sparql?default-graph-uri=&query=PREFIX+ontolex%3A+%3Chttp%3A%2F%2Fwww.w3.org%2Fns%2Flemon%2Fontolex%23%3E%0D%0APREFIX+dbnary%3A+%3Chttp%3A%2F%2Fkaiko.getalp.org%2Fdbnary%23%3E%0D%0APREFIX+lexinfo%3A+%3Chttp%3A%2F%2Fwww.lexinfo.net%2Fontology%2F2.0%2Flexinfo%23%3E%0D%0APREFIX+rdf%3A+%3Chttp%3A%2F%2Fwww.w3.org%2F1999%2F02%2F22-rdf-syntax-ns%23%3E%0D%0APREFIX+olia%3A+%3Chttp%3A%2F%2Fpurl.org%2Folia%2Folia.owl%23%3E%0D%0APREFIX+skos%3A+%3Chttp%3A%2F%2Fwww.w3.org%2F2004%2F02%2Fskos%2Fcore%23%3E%0D%0APREFIX+terms%3A+++%3Chttp%3A%2F%2Fpurl.org%2Fdc%2Fterms%2F%3E%0D%0APREFIX+lime%3A+%3Chttp%3A%2F%2Fwww.w3.org%2Fns%2Flemon%2Flime%23%3E%0D%0APREFIX+vartrans%3A+++%3Chttp%3A%2F%2Fwww.w3.org%2Fns%2Flemon%2Fvartrans%23%3E%0D%0APREFIX+prot%3A+++%3Chttp%3A%2F%2Fproton.semanticweb.org%2Fprotonsys%23%3E%0D%0APREFIX+xsd%3A+%3Chttp%3A%2F%2Fwww.w3.org%2F2001%2FXMLSchema%23%3E%0D%0APREFIX+iso%3A+%3Chttp%3A%2F%2Flexvo.org%2Fid%2Fiso639-3%2F%3E%0D%0A%0D%0ASELECT+%0D%0A%28count%28%3Fp%29+as+%3Fcp%29%0D%0A%23distinct+%3Fp%0D%0AWHERE+%7B%0D%0A++%3Fs+a+%3Ft+.%0D%0A++%3Fs+%3Fp+%3Fo+.%0D%0A++%3Fo+a+dbnary%3APage+.%0D%0A++%23+FILTER+NOT+EXISTS+%7B+%3Fo+a+rdf%3AProperty+%7D%0D%0A++%23+bind%28coalesce%28%3Fo1%2C%3F%29+as+%3Foo1%29%0D%0Afilter%28%3Fp%21%3Ddbnary%3Adescribes%29%0D%0Afilter%28%3Ft%3Ddbnary%3APage%29%0D%0Afilter%28%3Ft%21%3Dontolex%3ALexicalSens%29%0D%0A%0D%0A%7D%0D%0A%23+GROUP+BY+%3Fo+%0D%0Alimit+1000&format=text%2Fhtml&timeout=0&debug=on
  // Page to Sense or Entry: 0
  public class Page_Page {
    public int PageId { get; set; }
    public int ToPageId { get; set; }

    public byte NymType { get; set; }
  }

}
