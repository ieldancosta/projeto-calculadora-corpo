// Importações / Dependências
import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { supabase } from '../services/supabase';


const POSTS_POR_PAGINA = 10; // Limite de eficiência do Infinite Scroll

export function FeedPage() {
  const navigate = useNavigate(); /* Instancia o controlador de tráfego */
  const [postagens, setPostagens] = useState([]);
  const [novaPostagem, setNovaPostagem] = useState('');
  const [carregando, setCarregando] = useState(true);
  const [carregandoMais, setCarregandoMais] = useState(false); // Para o Infinite Scroll
  const [enviando, setEnviando] = useState(false);
  const [meuPerfil, setMeuPerfil] = useState(null);

  const [pagina, setPagina] = useState(0);
  const [temMais, setTemMais] = useState(true);

  useEffect(() => {
    carregarFeed(0);
    carregarMeuPerfil();
  }, []);

  // --- Lógica do Infinite Scroll ---
  useEffect(() => {
    const handleScroll = () => {
      // Verifica se o usuário chegou a 100px do final da página
      if (window.innerHeight + document.documentElement.scrollTop >= document.documentElement.offsetHeight - 100) {
        if (!carregando && !carregandoMais && temMais) {
          handleCarregarMais();
        }
      }
    };

    window.addEventListener('scroll', handleScroll);
    return () => window.removeEventListener('scroll', handleScroll); // Cleanup vital para evitar vazamento de memória
  }, [carregando, carregandoMais, temMais, pagina]);

  const carregarMeuPerfil = async () => {
    const { data: { user } } = await supabase.auth.getUser();
    if (user) {
      const { data } = await supabase.from('perfis').select('nome, username').eq('id', user.id).single();
      setMeuPerfil(data);
    }
  };

  const carregarFeed = async (pagAtual = 0) => {
    try {
      if (pagAtual > 0) setCarregandoMais(true);

      const de = pagAtual * POSTS_POR_PAGINA;
      const ate = de + POSTS_POR_PAGINA - 1;

      // A mágica do Supabase: Ele faz um "Join" automático com a tabela de perfis para pegar o nome e username!
      const { data, error } = await supabase
        .from('postagens')
        .select(`
          id,
          conteudo,
          criado_em,
          perfis ( nome, username )
        `)
        .order('criado_em', { ascending: false }) // Do mais novo para o mais velho
        .range(de, ate);

      if (error) throw error;

      if (data.length < POSTS_POR_PAGINA) setTemMais(false);
      else setTemMais(true);

      if (pagAtual === 0) {
        setPostagens(data);
      } else {
        setPostagens((prev) => [...prev, ...data]);
      }
    } catch (error) {
      console.error("Erro ao carregar feed:", error);
    } finally {
      setCarregando(false);
      setCarregandoMais(false);
    }
  };

  const handleCarregarMais = () => {
    if (carregandoMais || !temMais) return;
    const proximaPagina = pagina + 1;
    setPagina(proximaPagina);
    carregarFeed(proximaPagina);
  };

  const handlePostar = async (e) => {
    e.preventDefault();
    if (!novaPostagem.trim()) return;

    setEnviando(true);
    try {
      const { data: { user } } = await supabase.auth.getUser();

      const { error } = await supabase.from('postagens').insert([
        { perfil_id: user.id, conteudo: novaPostagem.trim() }
      ]);

      if (error) throw error;

      setNovaPostagem(''); // Limpa o campo
      setPagina(0);
      carregarFeed(0); // Atualiza a lista para mostrar a nova postagem

    } catch (error) {
      alert("Erro ao postar: " + error.message);
    } finally {
      setEnviando(false);
    }
  };

  // --- Design System ---
  const containerStyle = {
    backgroundColor: '#f1f5f9', minHeight: '100vh', display: 'flex', justifyContent: 'center', fontFamily: 'Arial, sans-serif', paddingBottom: '80px'
  };

  const feedWrapperStyle = {
    width: '100%', maxWidth: '600px', backgroundColor: '#fff', minHeight: '100vh', borderLeft: '1px solid #e2e8f0', borderRight: '1px solid #e2e8f0', padding: '20px'
  };

  const formStyle = {
    display: 'flex', flexDirection: 'column', gap: '10px', marginBottom: '30px', borderBottom: '1px solid #e2e8f0', paddingBottom: '20px'
  };

  const textareaStyle = {
    width: '100%', padding: '15px', borderRadius: '8px', border: '1px solid #cbd5e1', resize: 'none', fontSize: '16px', boxSizing: 'border-box'
  };

  const buttonStyle = {
    alignSelf: 'flex-end', padding: '10px 24px', backgroundColor: '#3b82f6', color: 'white', border: 'none', borderRadius: '20px', fontWeight: 'bold', cursor: enviando ? 'not-allowed' : 'pointer'
  };

  const postStyle = {
    padding: '15px 0', borderBottom: '1px solid #f1f5f9'
  };

  // Botão flutuante no canto inferior esquerdo
  const profileWidgetStyle = {
    position: 'fixed', bottom: '30px', left: '30px', backgroundColor: '#1e293b', color: 'white', padding: '15px 20px', borderRadius: '12px', cursor: 'pointer', boxShadow: '0 10px 15px -3px rgba(0, 0, 0, 0.1)', display: 'flex', flexDirection: 'column', gap: '5px'
  };

  return (
    <div style={containerStyle}>

      {/* Coluna Central do Feed */}
      <div style={feedWrapperStyle}>
        <h2 style={{ margin: '0 0 20px 0', color: '#0f172a' }}>Comunidade</h2>

        {/* Área de Criar Postagem */}
        <form onSubmit={handlePostar} style={formStyle}>
          <textarea
            rows="3"
            placeholder="Compartilhe seus resultados ou faça uma pergunta..."
            value={novaPostagem}
            onChange={(e) => setNovaPostagem(e.target.value)}
            style={textareaStyle}
            disabled={enviando}
          />
          <button type="submit" style={buttonStyle} disabled={enviando || !novaPostagem.trim()}>
            {enviando ? 'Enviando...' : 'Publicar'}
          </button>
        </form>

        {/* Lista de Postagens */}
        {carregando && pagina === 0 ? (
          <p style={{ textAlign: 'center', color: '#64748b' }}>Carregando postagens...</p>
        ) : postagens.length === 0 ? (
          <p style={{ textAlign: 'center', color: '#64748b' }}>Seja o primeiro a postar algo!</p>
        ) : (
          <>
            {postagens.map((post) => (
              <div key={post.id} style={postStyle}>
                <div style={{ display: 'flex', alignItems: 'center', gap: '8px', marginBottom: '8px' }}>
                  {/* O ?. garante que se perfis for null, o app não quebra */}
                  <strong style={{ color: '#0f172a', fontSize: '15px' }}>{post.perfis?.nome || 'Usuário Removido'}</strong>
                  <span style={{ color: '#64748b', fontSize: '14px' }}>
                    {post.perfis?.username ? `@${post.perfis.username}` : ''}
                  </span>
                </div>

                {/* Correção de UI: textAlign: 'left' e margin com 8px na esquerda para indentação */}
                <p style={{ margin: '0 0 0 8px', color: '#334155', fontSize: '15px', lineHeight: '1.5', textAlign: 'left' }}>
                  {post.conteudo}
                </p>
              </div>
            ))}

            {/* Feedback visual elegante ao rolar */}
            {carregandoMais && <p style={{ textAlign: 'center', color: '#94a3b8', marginTop: '20px' }}>Carregando mais postagens...</p>}
            {!temMais && postagens.length > 0 && <p style={{ textAlign: 'center', color: '#cbd5e1', marginTop: '20px', fontSize: '13px' }}>Você chegou ao fim do feed.</p>}
          </>
        )} {/* <- FECHAMENTO CORRIGIDO AQUI */}
      </div> {/* <- FECHAMENTO DA COLUNA CORRIGIDO AQUI */}

      {/* Widget do Perfil (Canto Inferior Esquerdo) */}
      {meuPerfil && (
        <div style={profileWidgetStyle} onClick={() => navigate('/perfil')}>
          <span style={{ fontSize: '14px', fontWeight: 'bold' }}>{meuPerfil.nome}</span>
          <span style={{ fontSize: '12px', color: '#94a3b8' }}>Ver meu metabolismo ➔</span>
        </div>
      )}

    </div>
  );
}
