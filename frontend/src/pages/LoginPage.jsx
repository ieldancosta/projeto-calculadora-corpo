import { useState } from 'react';
import { supabase } from '../services/supabase'; // Importando o nosso cliente configurado

export function LoginPage() {
  const [email, setEmail] = useState('');
  const [senha, setSenha] = useState('');
  const [erro, setErro] = useState('');
  const [carregando, setCarregando] = useState(false);

  // Função para Login com E-mail e Senha
  const handleLogin = async (e) => {
    e.preventDefault();
    setErro('');
    setCarregando(true);

    if (!email || !senha) {
      setErro('Por favor, preencha todos os campos.');
      setCarregando(false);
      return;
    }

    // Chamada mágica do Supabase
    const { data, error } = await supabase.auth.signInWithPassword({
      email: email,
      password: senha,
    });

    if (error) {
      setErro(error.message);
    } else {
      alert(`Bem-vindo(a)! Login realizado com sucesso.`);
      console.log('Dados do usuário:', data.user);
      // Aqui entrará o redirecionamento para o painel principal no futuro
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

  // --- Estilos ---
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

  return (
    <div style={containerStyle}>
      <form onSubmit={handleLogin} style={formStyle}>
        <h2 style={{ textAlign: 'center', margin: '0 0 10px 0', color: '#333' }}>Entrar no Sistema</h2>
        
        <div>
          <label style={{ display: 'block', marginBottom: '5px', fontSize: '14px' }}>E-mail</label>
          <input 
            type="email" 
            value={email}
            onChange={(e) => setEmail(e.target.value)} 
            placeholder="seu-email@exemplo.com"
            style={inputStyle}
            disabled={carregando}
          />
        </div>

        <div>
          <label style={{ display: 'block', marginBottom: '5px', fontSize: '14px' }}>Senha</label>
          <input 
            type="password" 
            value={senha}
            onChange={(e) => setSenha(e.target.value)} 
            placeholder="Digite sua senha"
            style={inputStyle}
            disabled={carregando}
          />
        </div>

        {erro && <p style={{ color: 'red', fontSize: '13px', margin: 0 }}>{erro}</p>}

        <button type="submit" style={buttonStyle} disabled={carregando}>
          {carregando ? 'Entrando...' : 'Acessar'}
        </button>

        <div style={{ textAlign: 'center', color: '#666', fontSize: '12px', margin: '5px 0' }}>ou</div>

        <button type="button" onClick={handleGoogleLogin} style={googleButtonStyle}>
          Entrar com Google
        </button>
      </form>
    </div>
  );
}