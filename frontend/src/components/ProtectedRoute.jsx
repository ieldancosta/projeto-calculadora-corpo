// Importações / Dependências
import { useState, useEffect } from 'react';
import { Navigate } from 'react-router-dom';
import { supabase } from '../services/supabase';


export function ProtectedRoute({ children }) {
  const [sessao, setSessao] = useState(null);
  const [carregando, setCarregando] = useState(true);

  useEffect(() => {
    // 1. Resolvemos a Promise inicial para checar a sessão atual imediatamente
    supabase.auth.getSession().then(({ data: { session } }) => {
      setSessao(session);
      setCarregando(false); // Só paramos de carregar quando a Promise é resolvida
    });

    // 2. Mantemos um vigia ativo (Callback) para mudanças em tempo real (ex: logout em outra aba)
    const { data: { subscription } } = supabase.auth.onAuthStateChange((_event, session) => {
      setSessao(session);
    });

    // 3. Limpeza do vigia quando o componente for desmontado
    return () => subscription.unsubscribe();
  }, []);

  // Enquanto a Promise do Supabase não resolve, seguramos a tela
  if (carregando) {
    return (
      <div style={{ display: 'flex', justifyContent: 'center', alignItems: 'center', height: '100vh', backgroundColor: '#f8fafc' }}>
        <h2>Verificando credenciais...</h2>
      </div>
    );
  }

  // Se tiver sessão, renderiza o componente filho (Dashboard/Quiz). Se não, redireciona para a raiz (Login)
  return sessao ? children : <Navigate to="/" />;
}
