// Importações / Dependências
import { useState, useEffect } from 'react';
import { supabase } from '../services/supabase'; /* Importando o nosso cliente configurado */
import { useNavigate, Link } from 'react-router-dom';


/* 1. O primeiro passo é bloquear o recarregamento da página */
/* 2. Definir como vai ser os métodos em sucesso */
/* 3. Validação de dados caso não preencher com e-mail ou senha (campos obrigatórios) */
/* 4. Método do supabase de Autenticação, onde é passado para ele para fazer a autenticação dos dados, com o login e senha. Se algo der errado, ele
retorna um erro, e os dados nulo. Se for um sucesso, o erro vai retornar nulo e os dados do usuário preenchidos */
/* 5. Se tiver uma mensagem de erro, ele exibe; Caso não, será autenticado e redirecionado para a página principal que quiser */

/* 6. Opção de login com o Google */
/* 7. CSS */
/* 8. HTML */


export function LoginPage() {
  const navigate = useNavigate(); /* Instancia o controlador de tráfego */
  const [email, setEmail] = useState('');
  const [senha, setSenha] = useState('');
  const [erro, setErro] = useState('');
  const [carregando, setCarregando] = useState(false);

  // --- CHECAGEM INICIAL DE SESSÃO ---
  useEffect(() => {
    const verificarSessaoExistente = async () => {
      // O getSession automaticamente lê a URL se o usuário estiver voltando do Google
      const { data: { session } } = await supabase.auth.getSession();

      if (session) {
        setCarregando(true);
        try {
          const { data: perfil, error: perfilError } = await supabase
            .from('perfis')
            .select('id')
            .eq('id', session.user.id)
            .single();

          if (perfilError || !perfil) {
            navigate('/onboarding');
          } else {
            navigate('/feed');
          }
        } catch (err) {
          console.error("Erro ao verificar perfil:", err);
        } finally {
          setCarregando(false);
        }
      }
    };

    verificarSessaoExistente();
  }, [navigate]);

  // Função para Login com E-mail e Senha
  const handleLogin = async (e) => {
    e.preventDefault(); /* Esse comando bloqueia o recarregamento da página, para que não seja perdido os dados na memória do React */
    setErro(''); /* Limpa erros de tentativas anteriores */
    setCarregando(true); /* Avisamos o React para para começar a carregar */

    if (!email || !senha) {
      setErro('Por favor, preencha todos os campos.'); /* Definimos uma mensagem de erro */
      setCarregando(false); /* Avisamos o React para não carregar */
      return;
    }

    // Chamada mágica do Supabase
    const { data, error } = await supabase.auth.signInWithPassword({
      email: email,
      password: senha,

      /* Para fins de informação: */
      /* O Supabase sempre devolve um objeto com essas duas propriedades: data (dados do usuário) e error (erro) */
      /* Se der erro (ex: senha errada), a variável error vem preenchida e data vem nula */
      /* Se der certo, error vem nulo e data traz as informações do usuário */
    });

    if (error) {
      setErro(error.message);
    } else {
      // O login deu certo! Agora checamos se ele já preencheu o quiz alguma vez
      const { data: perfil, error: perfilError } = await supabase
        .from('perfis')
        .select('id')
        .eq('id', data.user.id)
        .single(); // Tenta pegar uma linha única

      if (perfilError || !perfil) {
        // Se deu erro ou não achou linha, significa que é o primeiro acesso
        navigate('/onboarding');
      } else {
        // Se o perfil existe, vai direto para o painel/fórum principal
        navigate('/feed'); // Talvez eu mude futuramente essa rota
      }
    }

    setCarregando(false);
  };


  // Função para Autenticação com Google
  const handleGoogleLogin = async () => {
    setErro('');
    const { error } = await supabase.auth.signInWithOAuth({
      provider: 'google',
    });

    if (error) {
      setErro('Erro ao tentar logar com o Google: ' + error.message);
    }
  };


  // --- Estilos --- CSS
  const containerStyle = {
    display: 'flex', flexDirection: 'column', alignItems: 'center',
    justifyContent: 'center', height: '100vh', fontFamily: 'Arial, sans-serif',
    backgroundColor: '#f4f4f9'
  };

  const formStyle = {
    backgroundColor: '#fff', padding: '30px', borderRadius: '8px',
    boxShadow: '0 4px 6px rgba(0,0,0,0.1)', width: '300px', display: 'flex',
    flexDirection: 'column', gap: '15px'
  };

  const inputStyle = {
    width: '100%', padding: '8px', borderRadius: '4px',
    border: '1px solid #ccc', boxSizing: 'border-box'
  };

  const buttonStyle = {
    width: '100%', padding: '10px', backgroundColor: '#007bff',
    color: 'white', border: 'none', borderRadius: '4px',
    cursor: carregando ? 'not-allowed' : 'pointer', fontWeight: 'bold'
  };

  const googleButtonStyle = {
    ...buttonStyle,
    backgroundColor: '#db4437', // Vermelho característico do Google
    marginTop: '10px'
  };


  // Retorno para o usuário --- HTML
  return (
    <div style={containerStyle}>
      <form onSubmit={handleLogin} style={formStyle}>
        <h2 style={{ textAlign: 'center', margin: '0 0 10px 0', color: '#333' }}>Entrar no Sistema</h2>

        <div>
          {/* <label style={{ display: 'block', marginBottom: '5px', fontSize: '14px' }}>E-mail</label> */}
          <input
            type="email"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
            placeholder="Digite seu e-mail"
            style={inputStyle}
            disabled={carregando}
          />
        </div>

        <div>
          {/* <label style={{ display: 'block', marginBottom: '5px', fontSize: '14px' }}>Senha</label> */}
          <input
            type="password"
            value={senha}
            onChange={(e) => setSenha(e.target.value)}
            placeholder="Digite sua senha"
            style={inputStyle}
            disabled={carregando}
          />
        </div>

        {/* Para fins de informação: */}
        {/* O && no React funciona como um "Se" */}
        {/* Se a variável erro tiver algum texto dentro dela, então exiba esse parágrafo <p> vermelho na tela */}
        {/* Se não tiver erro, essa linha de código é invisível */}
        {erro && <p style={{ color: 'red', fontSize: '13px', margin: 0 }}>{erro}</p>}

        <button type="submit" style={buttonStyle} disabled={carregando}>
          {carregando ? 'Entrando...' : 'Acessar'}
        </button>

        <div style={{ textAlign: 'center', color: '#666', fontSize: '12px', margin: '5px 0' }}>ou</div>

        <button type="button" onClick={handleGoogleLogin} style={googleButtonStyle}>
          Entrar com Google
        </button>

        <div style={{ textAlign: 'center', marginTop: '10px', fontSize: '13px' }}>
          Não tem uma conta? <Link to="/cadastro" style={{ color: '#007bff', textDecoration: 'none', fontWeight: 'bold' }}>Cadastre-se</Link>
        </div>
      </form>
    </div>
  );
}